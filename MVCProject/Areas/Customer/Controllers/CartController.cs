using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;
using Utility;

namespace MVCProject.Areas.Customer.Controllers
{
    [Area("Customer"), Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartVM cart { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart = new()
            {
                Items = _unitOfWork.CartItem.GetAll(ci => ci.UserId == userId, "Product"),
                Order = new()
            };
            foreach (var item in cart.Items)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                cart.Order.Total += item.Price * item.Count;
            }
            return View(cart);
        }

        public IActionResult Summary()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart = new()
            {
                Items = _unitOfWork.CartItem.GetAll(i => i.UserId == userId, "Product")
            };
            cart.Order = new()
            {
                User = _unitOfWork.User.Get(userId),
            };
            cart.Order.Name = cart.Order.User.Name;
            cart.Order.Surname = cart.Order.User.Surname;
            cart.Order.PhoneNumber = cart.Order.User.PhoneNumber;
            cart.Order.Country = cart.Order.User.Country;
            cart.Order.State = cart.Order.User.State;
            cart.Order.City = cart.Order.User.City;
            cart.Order.Address = cart.Order.User.Address;
            cart.Order.PostalCode = cart.Order.User.PostalCode;
            foreach (var item in cart.Items)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                cart.Order.Total += item.Price * item.Count;
            }
            return View(cart);
        }

        [HttpPost, ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.Items = _unitOfWork.CartItem.GetAll(i => i.UserId == userId, "Product");
            cart.Order.Id = Guid.NewGuid().ToString();
            cart.Order.Date = DateTime.Now;
            cart.Order.UserId = userId;
            User user = _unitOfWork.User.Get(userId);
            foreach (var item in cart.Items)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                cart.Order.Total += item.Price * item.Count;
            }
            if (user.CompanyId == null || user.CompanyId == Guid.Empty.ToString())
            {
                cart.Order.Status = SD.StatusPending;
                cart.Order.PaymentStatus = SD.PaymentStatusPending;
            }
            else
            {
                cart.Order.Status = SD.StatusApproved;
                cart.Order.PaymentStatus = SD.PaymentStatusDelayedPayment;
            }
            _unitOfWork.Order.Add(cart.Order);
            _unitOfWork.Save();
            foreach (var item in cart.Items)
            {
                _unitOfWork.OrderItem.Add(new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = item.ProductId,
                    OrderId = cart.Order.Id,
                    Price = item.Price,
                    Count = item.Count
                });
                _unitOfWork.Save();
            }
            if (user.CompanyId == null || user.CompanyId == Guid.Empty.ToString())
            {
                string domain = "https://localhost:44364/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={cart.Order.Id}",
                    CancelUrl = domain + "Customer/Cart/Index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in cart.Items)
                {
                    SessionLineItemOptions sessionLineItem = new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.Order.UpdateStripePaymentId(cart.Order.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction(nameof(OrderConfirmation), new { id = cart.Order.Id });
        }

        public IActionResult OrderConfirmation(string id)
        {
            Order order = _unitOfWork.Order.Get(id, "User");
            if (order.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                SessionService service = new();
                Session session = service.Get(order.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Order.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.Order.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
                IEnumerable<CartItem> items = _unitOfWork.CartItem.GetAll(i => i.UserId == order.UserId);
                _unitOfWork.CartItem.RemoveRange(items);
                _unitOfWork.Save();
            }
            return View((object)id);
        }

        public IActionResult IncrementItemCount(string itemId)
        {
            if (ModelState.IsValid)
            {
                CartItem item = _unitOfWork.CartItem.Get(itemId);
                item.Count++;
                _unitOfWork.CartItem.Update(item);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementItemCount(string itemId)
        {
            if (ModelState.IsValid)
            {
                CartItem item = _unitOfWork.CartItem.Get(itemId);
                if (item.Count <= 1)
                {
                    _unitOfWork.CartItem.Remove(item);
                }
                else
                {
                    item.Count--;
                    _unitOfWork.CartItem.Update(item);
                }
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveItem(string itemId)
        {
            if (ModelState.IsValid)
            {
                CartItem item = _unitOfWork.CartItem.Get(itemId);
                _unitOfWork.CartItem.Remove(item);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(CartItem cartItem)
        {
            if (cartItem.Count < 50)
            {
                return cartItem.Product.Price;
            }
            if (cartItem.Count >= 50 && cartItem.Count < 100)
            {
                return cartItem.Product.Price50;
            }
            return cartItem.Product.Price100;
        }
    }
}

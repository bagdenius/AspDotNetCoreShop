using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string status)
        {
            return View();
        }

        public IActionResult Details(string? id)
        {
            OrderVM = new()
            {
                Order = _unitOfWork.Order.Get(id, "User"),
                Items = _unitOfWork.OrderItem.GetAll(i => i.OrderId == id, "Product")
            };
            return View(OrderVM);
        }

        [HttpPost, Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrder()
        {
            Order order = _unitOfWork.Order.Get(OrderVM.Order.Id);
            order.Name = OrderVM.Order.Name;
            order.Surname = OrderVM.Order.Surname;
            order.PhoneNumber = OrderVM.Order.PhoneNumber;
            order.Country = OrderVM.Order.Country;
            order.State = OrderVM.Order.State;
            order.City = OrderVM.Order.City;
            order.Address = OrderVM.Order.Address;
            order.PostalCode = OrderVM.Order.PostalCode;
            if (string.IsNullOrEmpty(OrderVM.Order.Carrier))
            {
                order.Carrier = OrderVM.Order.Carrier;
            }
            if (string.IsNullOrEmpty(OrderVM.Order.TrackingNumber))
            {
                order.TrackingNumber = OrderVM.Order.TrackingNumber;
            }
            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();
            TempData["success"] = "Order Updated Successfully";
            return RedirectToAction(nameof(Details), new { id = OrderVM.Order.Id });
        }

        [HttpPost, Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.Order.UpdateStatus(OrderVM.Order.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Order Status Updated Successfully";
            return RedirectToAction(nameof(Details), new { id = OrderVM.Order.Id });
        }

        [HttpPost, Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            Order order = _unitOfWork.Order.Get(OrderVM.Order.Id);
            order.Carrier = OrderVM.Order.Carrier;
            order.TrackingNumber = OrderVM.Order.TrackingNumber;
            order.Status = SD.StatusShipped;
            order.ShippingDate = DateTime.Now;
            if (order.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                order.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }
            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();
            TempData["success"] = "Order Shipped Successfully";
            return RedirectToAction(nameof(Details), new { id = OrderVM.Order.Id });
        }

        [HttpPost, Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            Order order = _unitOfWork.Order.Get(OrderVM.Order.Id);
            if (order.PaymentStatus == SD.PaymentStatusApproved)
            {
                RefundCreateOptions options = new()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.PaymentIntentId
                };
                RefundService service = new();
                Refund refund = service.Create(options);
                _unitOfWork.Order.UpdateStatus(order.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.Order.UpdateStatus(order.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "Order Canceled Successfully";
            return RedirectToAction(nameof(Details), new { id = OrderVM.Order.Id });
        }

        [HttpPost, Authorize(Roles = SD.Role_Company)]
        public IActionResult PayNow()
        {
            OrderVM.Order = _unitOfWork.Order.Get(o => o.Id == OrderVM.Order.Id, "User");
            OrderVM.Items = _unitOfWork.OrderItem.GetAll(i => i.OrderId == OrderVM.Order.Id, "Product");
            string domain = Request.Scheme + "://" + Request.Host.Value + "/"; ;
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Admin/Order/PaymentConfirmation?id={OrderVM.Order.Id}",
                CancelUrl = domain + $"Admin/Order/Details?id={OrderVM.Order.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in OrderVM.Items)
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
            _unitOfWork.Order.UpdateStripePaymentId(OrderVM.Order.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult PaymentConfirmation(string id)
        {
            Order order = _unitOfWork.Order.Get(id);
            if (order.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                SessionService service = new();
                Session session = service.Get(order.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Order.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.Order.UpdateStatus(id, order.Status, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View((object)id);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Order> orders;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orders = _unitOfWork.Order.GetAll(includeProperties: "User");
            }
            else
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                orders = _unitOfWork.Order.GetAll(o => o.UserId == userId, "User");
            }
            switch (status)
            {
                case "inprocess": orders = orders.Where(o => o.Status == SD.StatusInProcess); break;
                case "pending": orders = orders.Where(o => o.PaymentStatus == SD.PaymentStatusDelayedPayment); break;
                case "completed": orders = orders.Where(o => o.Status == SD.StatusShipped); break;
                case "approved": orders = orders.Where(o => o.Status == SD.StatusApproved); break;
                default: break;
            }
            return Json(new { data = orders });
        }

        #endregion
    }
}

using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using System.Security.Claims;

namespace MVCProject.Areas.Customer.Controllers
{
    [Area("Customer"), Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartVM cart;

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
                Items = _unitOfWork.CartItem.GetAll(ci => ci.UserId == userId, "Product")
            };
            foreach (var item in cart.Items)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                cart.OrderTotal += item.Price * item.Count;
            }
            return View(cart);
        }

        public IActionResult Summary()
        {
            return View();
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

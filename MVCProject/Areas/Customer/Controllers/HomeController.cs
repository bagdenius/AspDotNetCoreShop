using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Security.Claims;
using Utility;

namespace MVCProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            Claim? userNameIdentifierClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            if (userNameIdentifierClaim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.CartItem.GetAll(i => i.UserId == userNameIdentifierClaim.Value).Count());
            }
            return View(products);
        }

        public IActionResult Details(string productId)
        {
            if (ModelState.IsValid && productId != Guid.Empty.ToString())
            {
                CartItem item = new()
                {
                    Product = _unitOfWork.Product.Get(productId, "Category"),
                    Count = 1,
                    ProductId = productId
                };
                return View(item);
            }
            return NotFound();
        }

        [HttpPost, Authorize]
        public IActionResult Details(CartItem item)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                item.UserId = userId;
                CartItem duplicateItem = _unitOfWork.CartItem.Get(i => i.UserId == userId && i.ProductId == item.ProductId);
                if (duplicateItem != null)
                {
                    duplicateItem.Count += item.Count;
                    _unitOfWork.CartItem.Update(duplicateItem);
                    TempData["success"] = "Product was updated in cart";
                }
                else
                {
                    item.Id = Guid.NewGuid().ToString();
                    _unitOfWork.CartItem.Add(item);
                    TempData["success"] = "Product was added to cart";
                }
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.CartItem.GetAll(i => i.UserId == userId).Count());
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Security.Claims;

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
            return View(products);
        }

        public IActionResult Details(string id)
        {
            if (ModelState.IsValid && id != Guid.Empty.ToString())
            {
                ShoppingCart shoppingCart = new()
                {
                    Product = _unitOfWork.Product.Get(id, "Category"),
                    ProductId = id,
                    Count = 1
                };
                return View(shoppingCart);
            }
            return NotFound();
        }

        [HttpPost, Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                shoppingCart.UserId = userId;
                ShoppingCart duplicateCart = _unitOfWork.ShoppingCart
                    .Get(sc => sc.UserId == userId && sc.ProductId == shoppingCart.ProductId);
                if (duplicateCart != null)
                {
                    duplicateCart.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(duplicateCart);
                }
                else
                {
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                _unitOfWork.Save();
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

using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product
                .GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Upsert(string? id)
        {
            if (ModelState.IsValid)
            {
                ProductVM productVM = new()
                {
                    Product = new Product(),
                    CategoryList = _unitOfWork.Category.GetAll()
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        })
                };
                if (id != null)
                {
                    productVM.Product = _unitOfWork.Product.Get(id);
                }
                return View(productVM);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }
                if (productVM.Product.Id == Guid.Empty.ToString())
                {
                    productVM.Product.Id = Guid.NewGuid().ToString();
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Product> products = _unitOfWork.Product
                    .GetAll(includeProperties: "Category");
                return Json(new { data = products });
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid && id != Guid.Empty.ToString())
            {
                Product product = _unitOfWork.Product.Get(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete successful" });
            }
            return NotFound();
        }

        #endregion
    }
}

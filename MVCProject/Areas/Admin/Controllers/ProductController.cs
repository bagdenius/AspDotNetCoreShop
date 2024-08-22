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
                productVM.Product = _unitOfWork.Product.Get(id, "Images");
            }
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IEnumerable<IFormFile> files)
        {
            if (productVM.Product.Id == null)
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
            // images upload
            if (files != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                foreach (var file in files)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\products\product-" + productVM.Product.Id;
                    string fullProductPath = Path.Combine(wwwRootPath, productPath);
                    if (!Directory.Exists(fullProductPath))
                    {
                        Directory.CreateDirectory(fullProductPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(fullProductPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductImage image = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Url = @"\" + productPath + @"\" + filename,
                        ProductId = productVM.Product.Id
                    };
                    if (productVM.Product.Images == null)
                    {
                        productVM.Product.Images = new List<ProductImage>();
                    }
                    productVM.Product.Images.Add(image);
                }
                _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteImage(string id)
        {
            ProductImage image = _unitOfWork.ProductImage.Get(id);
            if (image != null && !string.IsNullOrEmpty(image.Url))
            {
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.Url.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitOfWork.ProductImage.Remove(image);
                _unitOfWork.Save();
                TempData["success"] = "Image deleted successfully";
            }
            return RedirectToAction(nameof(Upsert), new { id = image.ProductId });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            Product product = _unitOfWork.Product.Get(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string fullProductPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);
            if (Directory.Exists(fullProductPath))
            {
                string[] filePaths = Directory.GetFiles(fullProductPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(fullProductPath);
            }
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully" });
        }

        #endregion
    }
}

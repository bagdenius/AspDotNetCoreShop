using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            if (ModelState.IsValid && id != Guid.Empty)
            {
                Category category = _unitOfWork.Category.Get(id);
                return View(category);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Category.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid && id != Guid.Empty)
            {
                Category category = _unitOfWork.Category.Get(id);
                return View(category);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid id)
        {
            if (ModelState.IsValid && id != Guid.Empty)
            {
                Category category = _unitOfWork.Category.Get(id);
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

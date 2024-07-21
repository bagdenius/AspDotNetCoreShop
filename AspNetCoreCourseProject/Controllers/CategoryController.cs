using Data.Database;
using Data.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace MVCProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repository;
        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _repository.GetAll();

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
                _repository.Add(category);
                _repository.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty || !ModelState.IsValid)
            {
                return NotFound();
            }
            Category? category = _repository.Get(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(category);
                _repository.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty || !ModelState.IsValid)
            {
                return NotFound();
            }
            Category? category = _repository.Get(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid id)
        {
            Category? category = _repository.Get(id);
            if (category is null)
            {
                return NotFound();
            }
            _repository.Remove(category);
            _repository.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

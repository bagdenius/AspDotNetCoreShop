using AspNetCoreCourseProject.Data;
using AspNetCoreCourseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCourseProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.Categories.ToList();

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
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(Guid? id)
        {
            if (id is null || id == Guid.Empty || !ModelState.IsValid)
            {
                return NotFound();
            }
            Category? category = _db.Categories.Find(id);
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
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(Guid? id)
        {
            if (id is null || id == Guid.Empty || !ModelState.IsValid)
            {
                return NotFound();
            }
            Category? category = _db.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            Category? category = _db.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

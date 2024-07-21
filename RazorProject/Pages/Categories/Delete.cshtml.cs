using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Data;
using RazorProject.Models;

namespace RazorProject.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(Guid? id)
        {
            if (id is not null && id != Guid.Empty)
            {
                Category = _db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            Category? category = _db.Categories.Find(Category.Id);
            if (category is null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}

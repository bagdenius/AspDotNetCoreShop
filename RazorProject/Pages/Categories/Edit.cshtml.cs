using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Data;
using RazorProject.Models;

namespace RazorProject.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public EditModel(ApplicationDbContext db)
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
            if (ModelState.IsValid && Category is not null)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}

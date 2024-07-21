using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Data;
using RazorProject.Models;

namespace RazorProject.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(Category);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}

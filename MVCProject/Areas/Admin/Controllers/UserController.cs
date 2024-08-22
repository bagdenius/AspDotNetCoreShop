using Data.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            if (ModelState.IsValid)
            {
                string roleId = _db.UserRoles.FirstOrDefault(ur => ur.UserId == id).RoleId;
                UserVM userVM = new()
                {
                    User = _db.Users.Include(u => u.Company).FirstOrDefault(u => u.Id == id),
                    RoleList = _db.Roles.Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Name
                    }),
                    CompanyList = _db.Companies.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };
                userVM.User.Role = _db.Roles.FirstOrDefault(r => r.Id == roleId).Name;
                return View(userVM);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Edit(UserVM userVM)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == userVM.User.Id);
            string roleId = _db.UserRoles.FirstOrDefault(ur => ur.UserId == userVM.User.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(r => r.Id == roleId).Name;
            user.Name = userVM.User.Name;
            user.Surname = userVM.User.Surname;
            user.Country = userVM.User.Country;
            user.State = userVM.User.State;
            user.City = userVM.User.City;
            user.Address = userVM.User.Address;
            user.PostalCode = userVM.User.PostalCode;
            user.PhoneNumber = userVM.User.PhoneNumber;
            user.Role = userVM.User.Role;
            if (userVM.User.Role == SD.Role_Company)
            {
                user.CompanyId = userVM.User.CompanyId;
            }
            else
            {
                user.CompanyId = null;
            }
            _db.Users.Update(user);
            _db.SaveChanges();
            _userManager.RemoveFromRoleAsync(user, oldRole).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, userVM.User.Role).GetAwaiter().GetResult();
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            if (ModelState.IsValid)
            {
                IEnumerable<User> users = _db.Users.Include(u => u.Company).ToList();
                IEnumerable<IdentityUserRole<string>> userRoles = _db.UserRoles.ToList();
                IEnumerable<IdentityRole> roles = _db.Roles.ToList();
                foreach (var user in users)
                {
                    string roleId = userRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId;
                    user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                }
                return Json(new { data = users });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            if (ModelState.IsValid)
            {
                User user = _db.Users.Find(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Error while locking/unlocking the user" });
                }
                if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
                {
                    user.LockoutEnd = null;
                }
                else
                {
                    user.LockoutEnd = DateTime.Now.AddYears(1000);
                }
                _db.Users.Update(user);
                _db.SaveChanges();
                return Json(new { success = true, message = "User locked/unlocked successfully" });
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid && id != null)
            {
                User User = _db.Users.FirstOrDefault(u => u.Id == id);
                if (User == null)
                {
                    return Json(new { success = false, message = "Error while deleting user" });
                }
                _db.Users.Remove(User);
                _db.SaveChanges();
                return Json(new { success = true, message = "User deleted successfully" });
            }
            return NotFound();
        }

        #endregion
    }
}

using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            UserVM userVM = new()
            {
                User = _unitOfWork.User.Get(u => u.Id == id, "Company"),
                RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            userVM.User.Role = _userManager.GetRolesAsync(userVM.User).GetAwaiter().GetResult().FirstOrDefault();
            return View(userVM);
        }

        [HttpPost]
        public IActionResult Edit(UserVM userVM)
        {
            User user = _unitOfWork.User.Get(u => u.Id == userVM.User.Id);
            string oldRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
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
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
            _userManager.RemoveFromRoleAsync(user, oldRole).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, userVM.User.Role).GetAwaiter().GetResult();
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<User> users = _unitOfWork.User.GetAll(includeProperties: "Company").ToList();
            foreach (var user in users)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            }
            return Json(new { data = users });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            User user = _unitOfWork.User.Get(id);
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
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
            return Json(new { success = true, message = "User locked/unlocked successfully" });
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            User User = _unitOfWork.User.Get(id);
            if (User == null)
            {
                return Json(new { success = false, message = "Error while deleting user" });
            }
            _unitOfWork.User.Remove(User);
            _unitOfWork.Save();
            return Json(new { success = true, message = "User deleted successfully" });
        }

        #endregion
    }
}

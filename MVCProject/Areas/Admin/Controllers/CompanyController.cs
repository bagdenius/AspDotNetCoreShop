using Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> companies = _unitOfWork.Company.GetAll();
            return View(companies);
        }

        public IActionResult Upsert(string? id)
        {
            if (id == null)
            {
                return View(new Company());
            }
            Company company = _unitOfWork.Company.Get(id);
            return View(company);
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (company.Id == null)
            {
                company.Id = Guid.NewGuid().ToString();
                _unitOfWork.Company.Add(company);
                TempData["success"] = "Company created successfully";
            }
            else
            {
                _unitOfWork.Company.Update(company);
                TempData["success"] = "Company updated successfully";
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Company> companies = _unitOfWork.Company.GetAll();
                return Json(new { data = companies });
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid && id != null)
            {
                Company company = _unitOfWork.Company.Get(id);
                if (company == null)
                {
                    return Json(new { success = false, message = "Error while deleting company" });
                }
                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Company deleted successfully" });
            }
            return NotFound();
        }

        #endregion
    }
}

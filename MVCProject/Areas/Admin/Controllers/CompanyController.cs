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

        public IActionResult Upsert(Guid id)
        {
            if (ModelState.IsValid)
            {
                if (id == Guid.Empty)
                {
                    return View(new Company());
                }
                Company company = _unitOfWork.Company.Get(id);
                return View(company);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == Guid.Empty)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int id)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Company> companies = _unitOfWork.Company.GetAll();
                return Json(new { data = companies });
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                Company company = _unitOfWork.Company.Get(id);
                if (company == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }
                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete successful" });
            }
            return NotFound();
        }

        #endregion
    }
}

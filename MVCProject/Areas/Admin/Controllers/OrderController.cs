using Data.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using System.Diagnostics;
using Utility;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string status)
        {
            return View();
        }

        public IActionResult Details(string? id)
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Order> orders = _unitOfWork.Order.GetAll(includeProperties: "User");
            switch (status)
            {
                case "inprocess": orders = orders.Where(o => o.Status == SD.StatusInProcess); break;
                case "pending": orders = orders.Where(o => o.PaymentStatus == SD.PaymentStatusDelayedPayment); break;
                case "completed": orders = orders.Where(o => o.Status == SD.StatusShipped); break;
                case "approved": orders = orders.Where(o => o.Status == SD.StatusApproved); break;
                default: break;
            }
            return Json(new { data = orders });
        }

        #endregion
    }
}

using Data.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;

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

        public IActionResult Index()
        {
            IEnumerable<Order> orders = _unitOfWork.Order.GetAll();
            List<OrderVM> orderVMs = [];
            foreach (var order in orders)
            {
                orderVMs.Add(new()
                {
                    Order = order,
                    Items = _unitOfWork.OrderItem.GetAll(i => i.OrderId == order.Id)
                });
            }
            return View(orderVMs);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Order> orders = _unitOfWork.Order.GetAll(includeProperties: "User");
            return Json(new { data = orders });
        }

        #endregion
    }
}

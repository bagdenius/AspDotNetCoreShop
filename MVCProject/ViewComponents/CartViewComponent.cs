using Data.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utility;

namespace MVCProject.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Claim? userNameIdentifierClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            if (userNameIdentifierClaim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.CartItem.GetAll(i => i.UserId == userNameIdentifierClaim.Value).Count());
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}

using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class UserAdminController : Controller
    {
        private readonly UserBLL _userBLL;

        public UserAdminController(UserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        public IActionResult Index()
        {
            return View(_userBLL.GetAll());
        }

        [HttpPost]
        public IActionResult UpdateRole(int userId, string role)
        {
            var (success, message) = _userBLL.UpdateRole(userId, role);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction("Index");
        }
    }
}
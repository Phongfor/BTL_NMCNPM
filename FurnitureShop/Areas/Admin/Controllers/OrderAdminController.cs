using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class OrderAdminController : Controller
    {
        private readonly OrderBLL _orderBLL;

        public OrderAdminController(OrderBLL orderBLL)
        {
            _orderBLL = orderBLL;
        }

        public IActionResult Index()
        {
            return View(_orderBLL.GetAll());
        }

        public IActionResult Detail(int id)
        {
            var order = _orderBLL.GetByID(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            var (success, message) = _orderBLL.UpdateStatus(orderId, status);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction("Detail", new { id = orderId });
        }
    }
}
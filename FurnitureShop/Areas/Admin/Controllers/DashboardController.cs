using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class DashboardController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly OrderBLL _orderBLL;
        private readonly UserBLL _userBLL;
        private readonly CategoryBLL _categoryBLL;

        public DashboardController(ProductBLL productBLL, OrderBLL orderBLL,
                                    UserBLL userBLL, CategoryBLL categoryBLL)
        {
            _productBLL = productBLL;
            _orderBLL = orderBLL;
            _userBLL = userBLL;
            _categoryBLL = categoryBLL;
        }

        public IActionResult Index()
        {
            ViewBag.TotalProducts = _productBLL.GetAll().Count;
            ViewBag.TotalOrders = _orderBLL.GetTotalOrders();
            ViewBag.TotalRevenue = _orderBLL.GetTotalRevenue();
            ViewBag.TotalUsers = _userBLL.GetAll().Count(u => u.Role == "Customer");
            ViewBag.RecentOrders = _orderBLL.GetRecentOrders(5);
            ViewBag.TotalCategories = _categoryBLL.GetAll().Count;
            return View();
        }
    }
}
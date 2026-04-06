using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;

        public HomeController(ProductBLL productBLL, CategoryBLL categoryBLL)
        {
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
        }

        // GET: /
        public IActionResult Index()
        {
            ViewBag.Categories = _categoryBLL.GetAll();
            ViewBag.NewestProducts = _productBLL.GetNewest(8);
            ViewBag.DiscountedProducts = _productBLL.GetDiscounted(8);
            ViewBag.CartCount = SessionHelper.GetCart(HttpContext.Session).Sum(x => x.Quantity);
            return View();
        }
    }
}
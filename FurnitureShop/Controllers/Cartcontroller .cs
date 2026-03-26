using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Controllers
{
    public class CartController : Controller
    {
        private readonly CartBLL _cartBLL;
        private readonly ProductBLL _productBLL;

        public CartController(CartBLL cartBLL, ProductBLL productBLL)
        {
            _cartBLL = cartBLL;
            _productBLL = productBLL;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = SessionHelper.GetCart(HttpContext.Session);
            ViewBag.Total = _cartBLL.GetTotal(cart);
            ViewBag.CartCount = _cartBLL.GetTotalQuantity(cart);
            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            if (!SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Account");

            var product = _productBLL.GetByID(productId);
            if (product == null) return NotFound();

            var cart = SessionHelper.GetCart(HttpContext.Session);
            cart = _cartBLL.AddToCart(cart, product, quantity);
            SessionHelper.SetCart(HttpContext.Session, cart);

            TempData["Success"] = $"Đã thêm \"{product.ProductName}\" vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = SessionHelper.GetCart(HttpContext.Session);
            cart = _cartBLL.UpdateQuantity(cart, productId, quantity);
            SessionHelper.SetCart(HttpContext.Session, cart);
            return RedirectToAction("Index");
        }

        // POST: /Cart/Remove
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = SessionHelper.GetCart(HttpContext.Session);
            cart = _cartBLL.RemoveFromCart(cart, productId);
            SessionHelper.SetCart(HttpContext.Session, cart);
            return RedirectToAction("Index");
        }
    }
}
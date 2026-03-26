using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderBLL _orderBLL;
        private readonly CartBLL _cartBLL;

        public OrderController(OrderBLL orderBLL, CartBLL cartBLL)
        {
            _orderBLL = orderBLL;
            _cartBLL = cartBLL;
        }

        // GET: /Order/Checkout
        public IActionResult Checkout()
        {
            if (!SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Account");

            var cart = SessionHelper.GetCart(HttpContext.Session);
            if (cart.Count == 0) return RedirectToAction("Index", "Cart");

            ViewBag.Cart = cart;
            ViewBag.Total = _cartBLL.GetTotal(cart);
            return View();
        }

        // POST: /Order/Checkout
        [HttpPost]
        public IActionResult Checkout(string shipAddress, string? note)
        {
            if (!SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Account");

            var cart = SessionHelper.GetCart(HttpContext.Session);
            int userId = SessionHelper.GetUserID(HttpContext.Session)!.Value;

            var (success, message, orderId) = _orderBLL.CreateOrder(
                userId, shipAddress, note ?? "", cart);

            if (!success)
            {
                ViewBag.Error = message;
                ViewBag.Cart = cart;
                ViewBag.Total = _cartBLL.GetTotal(cart);
                return View();
            }

            // Xóa giỏ hàng sau khi đặt thành công
            SessionHelper.SetCart(HttpContext.Session, _cartBLL.ClearCart());

            return RedirectToAction("Success", new { id = orderId });
        }

        // GET: /Order/Success/5
        public IActionResult Success(int id)
        {
            var order = _orderBLL.GetByID(id);
            return View(order);
        }

        // GET: /Order/MyOrders
        public IActionResult MyOrders()
        {
            if (!SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Account");

            int userId = SessionHelper.GetUserID(HttpContext.Session)!.Value;
            var orders = _orderBLL.GetByUser(userId);
            return View(orders);
        }

        // GET: /Order/Detail/5
        public IActionResult Detail(int id)
        {
            if (!SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Account");

            var order = _orderBLL.GetByID(id);
            if (order == null) return NotFound();

            // Chỉ cho xem đơn của chính mình (trừ Admin)
            int userId = SessionHelper.GetUserID(HttpContext.Session)!.Value;
            if (order.UserID != userId && !SessionHelper.IsAdmin(HttpContext.Session))
                return Forbid();

            return View(order);
        }
    }
}
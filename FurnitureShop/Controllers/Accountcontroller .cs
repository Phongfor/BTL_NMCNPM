using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserBLL _userBLL;

        public AccountController(UserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            if (SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _userBLL.Login(email, password);
            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng.";
                return View();
            }

            // Lưu thông tin vào Session
            SessionHelper.SetUserID(HttpContext.Session, user.UserID);
            SessionHelper.SetUserName(HttpContext.Session, user.FullName);
            SessionHelper.SetRole(HttpContext.Session, user.Role);

            // Phân quyền chuyển hướng
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            if (SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password,
                                       string confirmPassword, string phone, string address)
        {
            var (success, message) = _userBLL.Register(
                fullName, email, password, confirmPassword, phone, address);

            if (!success)
            {
                ViewBag.Error = message;
                return View();
            }

            TempData["Success"] = message;
            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            SessionHelper.Clear(HttpContext.Session);
            return RedirectToAction("Login");
        }
    }
}
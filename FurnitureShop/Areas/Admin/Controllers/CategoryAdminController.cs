using FurnitureShop.BLL;
using FurnitureShop.DTO;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class CategoryAdminController : Controller
    {
        private readonly CategoryBLL _categoryBLL;

        public CategoryAdminController(CategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
        }

        public IActionResult Index()
        {
            return View(_categoryBLL.GetAll());
        }

        [HttpPost]
        public IActionResult Create(CategoryDTO category)
        {
            var (success, message) = _categoryBLL.Insert(category);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(CategoryDTO category)
        {
            var (success, message) = _categoryBLL.Update(category);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var (success, message) = _categoryBLL.Delete(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction("Index");
        }
    }
}
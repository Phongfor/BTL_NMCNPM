using FurnitureShop.BLL;
using FurnitureShop.DTO;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class ProductAdminController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;

        public ProductAdminController(ProductBLL productBLL, CategoryBLL categoryBLL)
        {
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
        }

        // GET: /Admin/ProductAdmin
        public IActionResult Index(string? keyword)
        {
            var products = string.IsNullOrWhiteSpace(keyword)
                ? _productBLL.GetAll()
                : _productBLL.Search(keyword, null, null, null, null);
            ViewBag.Keyword = keyword;
            return View(products);
        }

        // GET: Thêm mới (không có id)
        public IActionResult Edit()
        {
            ViewBag.Categories = _categoryBLL.GetAll();
            return View(new ProductDTO());
        }

        // GET: Sửa (có id)
        [HttpGet]
        [Route("Admin/ProductAdmin/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var product = _productBLL.GetByID(id);
            if (product == null) return NotFound();
            ViewBag.Categories = _categoryBLL.GetAll();
            return View(product);
        }

        // POST: Lưu (thêm hoặc sửa)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO product, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var folder = Path.Combine(Directory.GetCurrentDirectory(),
                                            "wwwroot/images/products");
                Directory.CreateDirectory(folder);
                var savePath = Path.Combine(folder, fileName);
                using var stream = new FileStream(savePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                product.ImageURL = "/images/products/" + fileName;
            }

            if (product.ProductID == 0)
            {
                var (success, message) = _productBLL.Insert(product);
                TempData[success ? "Success" : "Error"] = message;
            }
            else
            {
                var (success, message) = _productBLL.Update(product);
                TempData[success ? "Success" : "Error"] = message;
            }

            return RedirectToAction("Index");
        }

        // POST: /Admin/ProductAdmin/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var (success, message) = _productBLL.Delete(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction("Index");
        }
    }
}
using FurnitureShop.BLL;
using FurnitureShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private const int PageSize = 12;

        public ProductController(ProductBLL productBLL, CategoryBLL categoryBLL)
        {
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
        }

        // GET: /Product
        public IActionResult Index(int? categoryId, string? keyword,
                                    decimal? minPrice, decimal? maxPrice,
                                    string? sortBy, int page = 1)
        {
            var products = _productBLL.Search(keyword, categoryId, minPrice, maxPrice, sortBy);

            // Phân trang
            int totalItems = products.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            var paged = products.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Products = paged;
            ViewBag.Categories = _categoryBLL.GetAll();
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;
            ViewBag.Keyword = keyword;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            ViewBag.CartCount = SessionHelper.GetCart(HttpContext.Session).Sum(x => x.Quantity);

            return View();
        }

        // GET: /Product/Detail/5
        public IActionResult Detail(int id)
        {
            var product = _productBLL.GetByID(id);
            if (product == null) return NotFound();

            ViewBag.CartCount = SessionHelper.GetCart(HttpContext.Session).Sum(x => x.Quantity);
            return View(product);
        }
    }
}
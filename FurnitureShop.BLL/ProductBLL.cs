using FurnitureShop.DAL;
using FurnitureShop.DTO;

namespace FurnitureShop.BLL
{
    public class ProductBLL
    {
        private readonly ProductDAL _dal;

        public ProductBLL(string connectionString)
        {
            _dal = new ProductDAL(connectionString);
        }

        public List<ProductDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public ProductDTO? GetByID(int id)
        {
            if (id <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ.");
            return _dal.GetByID(id);
        }

        public List<ProductDTO> GetByCategory(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException("ID danh mục không hợp lệ.");
            return _dal.GetByCategory(categoryId);
        }

        // Tìm kiếm + lọc + sắp xếp — xử lý tại BLL
        public List<ProductDTO> Search(string? keyword, int? categoryId,
                                       decimal? minPrice, decimal? maxPrice,
                                       string? sortBy)
        {
            var list = _dal.Search(keyword, categoryId, minPrice, maxPrice);

            list = sortBy switch
            {
                "price_asc" => list.OrderBy(p => p.SalePrice).ToList(),
                "price_desc" => list.OrderByDescending(p => p.SalePrice).ToList(),
                "name_asc" => list.OrderBy(p => p.ProductName).ToList(),
                "newest" => list.OrderByDescending(p => p.CreatedDate).ToList(),
                _ => list.OrderByDescending(p => p.CreatedDate).ToList()
            };

            return list;
        }

        // Lấy sản phẩm có giảm giá cho trang chủ
        public List<ProductDTO> GetDiscounted(int top = 8)
        {
            return _dal.GetAll()
                       .Where(p => p.Discount > 0 && p.IsActive)
                       .OrderByDescending(p => p.Discount)
                       .Take(top)
                       .ToList();
        }

        // Lấy sản phẩm mới nhất cho trang chủ
        public List<ProductDTO> GetNewest(int top = 8)
        {
            return _dal.GetAll()
                       .Where(p => p.IsActive)
                       .OrderByDescending(p => p.CreatedDate)
                       .Take(top)
                       .ToList();
        }

        public (bool Success, string Message) Insert(ProductDTO p)
        {
            if (string.IsNullOrWhiteSpace(p.ProductName))
                return (false, "Tên sản phẩm không được để trống.");
            if (p.Price <= 0)
                return (false, "Giá sản phẩm phải lớn hơn 0.");
            if (p.CategoryID <= 0)
                return (false, "Vui lòng chọn danh mục.");
            if (p.Discount < 0 || p.Discount > 100)
                return (false, "Giảm giá phải từ 0 đến 100%.");

            bool result = _dal.Insert(p);
            return result
                ? (true, "Thêm sản phẩm thành công.")
                : (false, "Thêm sản phẩm thất bại.");
        }

        public (bool Success, string Message) Update(ProductDTO p)
        {
            if (p.ProductID <= 0)
                return (false, "ID sản phẩm không hợp lệ.");
            if (string.IsNullOrWhiteSpace(p.ProductName))
                return (false, "Tên sản phẩm không được để trống.");
            if (p.Price <= 0)
                return (false, "Giá sản phẩm phải lớn hơn 0.");
            if (p.Discount < 0 || p.Discount > 100)
                return (false, "Giảm giá phải từ 0 đến 100%.");

            bool result = _dal.Update(p);
            return result
                ? (true, "Cập nhật sản phẩm thành công.")
                : (false, "Cập nhật sản phẩm thất bại.");
        }

        public (bool Success, string Message) Delete(int id)
        {
            if (id <= 0) return (false, "ID sản phẩm không hợp lệ.");
            try
            {
                bool result = _dal.Delete(id);
                return result
                    ? (true, "Xóa sản phẩm thành công.")
                    : (false, "Xóa sản phẩm thất bại.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi: " + ex.Message);
            }
        }
    }
}
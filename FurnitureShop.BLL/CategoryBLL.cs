using FurnitureShop.DAL;
using FurnitureShop.DTO;

namespace FurnitureShop.BLL
{
    public class CategoryBLL
    {
        private readonly CategoryDAL _dal;

        public CategoryBLL(string connectionString)
        {
            _dal = new CategoryDAL(connectionString);
        }

        public List<CategoryDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public CategoryDTO? GetByID(int id)
        {
            if (id <= 0) throw new ArgumentException("ID danh mục không hợp lệ.");
            return _dal.GetByID(id);
        }

        public (bool Success, string Message) Insert(CategoryDTO c)
        {
            if (string.IsNullOrWhiteSpace(c.CategoryName))
                return (false, "Tên danh mục không được để trống.");

            bool result = _dal.Insert(c);
            return result
                ? (true, "Thêm danh mục thành công.")
                : (false, "Thêm danh mục thất bại.");
        }

        public (bool Success, string Message) Update(CategoryDTO c)
        {
            if (c.CategoryID <= 0)
                return (false, "ID danh mục không hợp lệ.");
            if (string.IsNullOrWhiteSpace(c.CategoryName))
                return (false, "Tên danh mục không được để trống.");

            bool result = _dal.Update(c);
            return result
                ? (true, "Cập nhật danh mục thành công.")
                : (false, "Cập nhật danh mục thất bại.");
        }

        public (bool Success, string Message) Delete(int id)
        {
            if (id <= 0) return (false, "ID danh mục không hợp lệ.");
            try
            {
                bool result = _dal.Delete(id);
                return result
                    ? (true, "Xóa danh mục thành công.")
                    : (false, "Xóa danh mục thất bại.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE") || ex.Message.Contains("constraint"))
                    return (false, "Không thể xóa! Danh mục này đang có sản phẩm.");
                return (false, "Lỗi: " + ex.Message);
            }
        }
    }
}
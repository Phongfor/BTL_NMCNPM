using Microsoft.Data.SqlClient;
using System.Data;
using FurnitureShop.DTO;

namespace FurnitureShop.DAL
{
    public class ProductDAL
    {
        private readonly DBConnection _db;

        public ProductDAL(string connectionString)
        {
            _db = new DBConnection(connectionString);
        }

        // Map dữ liệu từ SqlDataReader sang ProductDTO
        private ProductDTO MapProduct(SqlDataReader dr)
        {
            return new ProductDTO
            {
                ProductID = (int)dr["ProductID"],
                ProductName = dr["ProductName"].ToString()!,
                CategoryID = (int)dr["CategoryID"],
                CategoryName = dr["CategoryName"]?.ToString() ?? "",
                Price = (decimal)dr["Price"],
                Discount = dr["Discount"] != DBNull.Value ? (int)dr["Discount"] : 0,
                Stock = dr["Stock"] != DBNull.Value ? (int)dr["Stock"] : 0,
                Description = dr["Description"]?.ToString() ?? "",
                ImageURL = dr["ImageURL"]?.ToString() ?? "",
                IsActive = dr["IsActive"] != DBNull.Value && (bool)dr["IsActive"],
                CreatedDate = dr["CreatedDate"] != DBNull.Value
                               ? (DateTime)dr["CreatedDate"] : DateTime.Now
            };
        }

        public List<ProductDTO> GetAll()
        {
            var list = new List<ProductDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetAllProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read()) list.Add(MapProduct(dr));
            return list;
        }

        public ProductDTO? GetByID(int id)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetProductByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            conn.Open();
            var dr = cmd.ExecuteReader();
            return dr.Read() ? MapProduct(dr) : null;
        }

        public List<ProductDTO> GetByCategory(int categoryId)
        {
            var list = new List<ProductDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetProductsByCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read()) list.Add(MapProduct(dr));
            return list;
        }

        public List<ProductDTO> Search(string keyword, int? categoryId,
                                       decimal? minPrice, decimal? maxPrice)
        {
            var list = new List<ProductDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_SearchProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Keyword", (object?)keyword ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryID", (object?)categoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MinPrice", (object?)minPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaxPrice", (object?)maxPrice ?? DBNull.Value);
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read()) list.Add(MapProduct(dr));
            return list;
        }

        public bool Insert(ProductDTO p)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_InsertProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
            cmd.Parameters.AddWithValue("@CategoryID", p.CategoryID);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.Parameters.AddWithValue("@Discount", p.Discount);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@Description", p.Description);
            cmd.Parameters.AddWithValue("@ImageURL", p.ImageURL);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(ProductDTO p)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_UpdateProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", p.ProductID);
            cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
            cmd.Parameters.AddWithValue("@CategoryID", p.CategoryID);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.Parameters.AddWithValue("@Discount", p.Discount);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@Description", p.Description);
            cmd.Parameters.AddWithValue("@ImageURL", p.ImageURL);
            cmd.Parameters.AddWithValue("@IsActive", p.IsActive);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // Xóa mềm: chỉ set IsActive = 0
        public bool Delete(int id)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_DeleteProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
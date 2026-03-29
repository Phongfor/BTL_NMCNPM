using Microsoft.Data.SqlClient;
using System.Data;
using FurnitureShop.DTO;

namespace FurnitureShop.DAL
{
    public class CategoryDAL
    {
        private readonly DBConnection _db;

        public CategoryDAL(string connectionString)
        {
            _db = new DBConnection(connectionString);
        }

        public List<CategoryDTO> GetAll()
        {
            var list = new List<CategoryDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetAllCategories", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new CategoryDTO
                {
                    CategoryID = (int)dr["CategoryID"],
                    CategoryName = dr["CategoryName"].ToString()!,
                    Description = dr["Description"]?.ToString() ?? "",
                    ImageURL = dr["ImageURL"]?.ToString() ?? ""
                });
            }
            return list;
        }

        public CategoryDTO? GetByID(int id)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetCategoryByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", id);
            conn.Open();
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new CategoryDTO
                {
                    CategoryID = (int)dr["CategoryID"],
                    CategoryName = dr["CategoryName"].ToString()!,
                    Description = dr["Description"]?.ToString() ?? "",
                    ImageURL = dr["ImageURL"]?.ToString() ?? ""
                };
            }
            return null;
        }

        public bool Insert(CategoryDTO c)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_InsertCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryName", c.CategoryName);
            cmd.Parameters.AddWithValue("@Description", c.Description);
            cmd.Parameters.AddWithValue("@ImageURL", c.ImageURL);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(CategoryDTO c)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_UpdateCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", c.CategoryID);
            cmd.Parameters.AddWithValue("@CategoryName", c.CategoryName);
            cmd.Parameters.AddWithValue("@Description", c.Description);
            cmd.Parameters.AddWithValue("@ImageURL", c.ImageURL);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_DeleteCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
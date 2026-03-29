using Microsoft.Data.SqlClient;
using System.Data;
using FurnitureShop.DTO;

namespace FurnitureShop.DAL
{
    public class UserDAL
    {
        private readonly DBConnection _db;

        public UserDAL(string connectionString)
        {
            _db = new DBConnection(connectionString);
        }

        private UserDTO MapUser(SqlDataReader dr)
        {
            return new UserDTO
            {
                UserID = (int)dr["UserID"],
                FullName = dr["FullName"].ToString()!,
                Email = dr["Email"].ToString()!,
                Password = dr["Password"].ToString()!,
                Phone = dr["Phone"]?.ToString() ?? "",
                Address = dr["Address"]?.ToString() ?? "",
                Role = dr["Role"]?.ToString() ?? "Customer",
                CreatedDate = dr["CreatedDate"] != DBNull.Value
                              ? (DateTime)dr["CreatedDate"] : DateTime.Now
            };
        }

        // Đăng nhập: trả về UserDTO nếu đúng, null nếu sai
        public UserDTO? Login(string email, string passwordHash)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_Login", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", passwordHash);
            conn.Open();
            var dr = cmd.ExecuteReader();
            return dr.Read() ? MapUser(dr) : null;
        }

        // Đăng ký: trả về true nếu thành công
        public bool Register(UserDTO u)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_RegisterUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FullName", u.FullName);
            cmd.Parameters.AddWithValue("@Email", u.Email);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Address", u.Address);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // Kiểm tra email đã tồn tại chưa
        public bool EmailExists(string email)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_CheckEmailExists", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
            conn.Open();
            var result = cmd.ExecuteScalar();
            return result != null && (int)result > 0;
        }

        public List<UserDTO> GetAll()
        {
            var list = new List<UserDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetAllUsers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read()) list.Add(MapUser(dr));
            return list;
        }

        public bool UpdateRole(int userId, string role)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_UpdateUserRole", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@Role", role);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
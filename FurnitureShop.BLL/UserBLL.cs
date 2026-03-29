using System.Security.Cryptography;
using System.Text;
using FurnitureShop.DAL;
using FurnitureShop.DTO;

namespace FurnitureShop.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _dal;

        public UserBLL(string connectionString)
        {
            _dal = new UserDAL(connectionString);
        }

        // Hash mật khẩu bằng MD5
        public static string HashPassword(string password)
        {
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        // Đăng nhập — trả về UserDTO nếu thành công, null nếu sai
        public UserDTO? Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            string passwordHash = HashPassword(password);
            return _dal.Login(email.Trim().ToLower(), passwordHash);
        }

        // Đăng ký — trả về (Success, Message)
        public (bool Success, string Message) Register(string fullName, string email,
                                                        string password, string confirmPassword,
                                                        string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return (false, "Họ tên không được để trống.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return (false, "Email không hợp lệ.");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return (false, "Mật khẩu phải có ít nhất 6 ký tự.");
            if (password != confirmPassword)
                return (false, "Mật khẩu xác nhận không khớp.");

            // Kiểm tra email đã tồn tại
            if (_dal.EmailExists(email.Trim().ToLower()))
                return (false, "Email này đã được đăng ký.");

            var user = new UserDTO
            {
                FullName = fullName.Trim(),
                Email = email.Trim().ToLower(),
                Password = HashPassword(password),
                Phone = phone?.Trim() ?? "",
                Address = address?.Trim() ?? "",
                Role = "Customer"
            };

            bool result = _dal.Register(user);
            return result
                ? (true, "Đăng ký thành công! Vui lòng đăng nhập.")
                : (false, "Đăng ký thất bại, vui lòng thử lại.");
        }

        public List<UserDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public (bool Success, string Message) UpdateRole(int userId, string role)
        {
            if (userId <= 0) return (false, "ID người dùng không hợp lệ.");
            if (role != "Admin" && role != "Customer")
                return (false, "Role không hợp lệ.");

            bool result = _dal.UpdateRole(userId, role);
            return result
                ? (true, "Cập nhật quyền thành công.")
                : (false, "Cập nhật quyền thất bại.");
        }
    }
}
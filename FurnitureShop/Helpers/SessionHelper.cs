using System.Text.Json;
using FurnitureShop.DTO;

namespace FurnitureShop.Helpers
{
    // Helper lưu/đọc Session tiện lợi hơn
    public static class SessionHelper
    {
        // Lưu object vào Session dưới dạng JSON
        public static void SetObject<T>(ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Đọc object từ Session
        public static T? GetObject<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        // --- Các shortcut thường dùng ---

        public static void SetUserID(ISession session, int userId)
            => session.SetInt32("UserID", userId);

        public static int? GetUserID(ISession session)
            => session.GetInt32("UserID");

        public static void SetUserName(ISession session, string name)
            => session.SetString("FullName", name);

        public static string? GetUserName(ISession session)
            => session.GetString("FullName");

        public static void SetRole(ISession session, string role)
            => session.SetString("Role", role);

        public static string? GetRole(ISession session)
            => session.GetString("Role");

        public static bool IsLoggedIn(ISession session)
            => session.GetInt32("UserID") != null;

        public static bool IsAdmin(ISession session)
            => session.GetString("Role") == "Admin";

        // Giỏ hàng
        public static List<CartItemDTO> GetCart(ISession session)
            => GetObject<List<CartItemDTO>>(session, "Cart") ?? new List<CartItemDTO>();

        public static void SetCart(ISession session, List<CartItemDTO> cart)
            => SetObject(session, "Cart", cart);

        // Đăng xuất
        public static void Clear(ISession session)
            => session.Clear();
    }
}
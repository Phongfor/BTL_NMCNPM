using FurnitureShop.DAL;
using FurnitureShop.DTO;

namespace FurnitureShop.BLL
{
    public class OrderBLL
    {
        private readonly OrderDAL _dal;

        public OrderBLL(string connectionString)
        {
            _dal = new OrderDAL(connectionString);
        }

        // Tạo đơn hàng từ giỏ hàng
        public (bool Success, string Message, int OrderID) CreateOrder(
            int userId, string shipAddress, string note,
            List<CartItemDTO> cartItems)
        {
            if (userId <= 0)
                return (false, "Vui lòng đăng nhập để đặt hàng.", 0);
            if (string.IsNullOrWhiteSpace(shipAddress))
                return (false, "Vui lòng nhập địa chỉ giao hàng.", 0);
            if (cartItems == null || cartItems.Count == 0)
                return (false, "Giỏ hàng trống.", 0);

            var order = new OrderDTO
            {
                UserID = userId,
                ShipAddress = shipAddress.Trim(),
                Note = note?.Trim() ?? "",
                TotalAmount = cartItems.Sum(x => x.SubTotal),
                Status = "Chờ xác nhận",
                OrderDetails = cartItems.Select(item => new OrderDetailDTO
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                }).ToList()
            };

            int newOrderId = _dal.CreateOrder(order);
            return newOrderId > 0
                ? (true, "Đặt hàng thành công!", newOrderId)
                : (false, "Đặt hàng thất bại, vui lòng thử lại.", 0);
        }

        public List<OrderDTO> GetByUser(int userId)
        {
            if (userId <= 0) throw new ArgumentException("ID người dùng không hợp lệ.");
            return _dal.GetByUser(userId);
        }

        public List<OrderDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public OrderDTO? GetByID(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("ID đơn hàng không hợp lệ.");
            return _dal.GetByID(orderId);
        }

        public (bool Success, string Message) UpdateStatus(int orderId, string status)
        {
            var validStatuses = new[]
            {
                "Chờ xác nhận", "Đang xử lý",
                "Đang giao",    "Đã giao", "Hủy"
            };

            if (!validStatuses.Contains(status))
                return (false, "Trạng thái đơn hàng không hợp lệ.");

            bool result = _dal.UpdateStatus(orderId, status);
            return result
                ? (true, "Cập nhật trạng thái thành công.")
                : (false, "Cập nhật trạng thái thất bại.");
        }

        // Thống kê nhanh cho Admin Dashboard
        public decimal GetTotalRevenue()
        {
            return _dal.GetAll()
                       .Where(o => o.Status == "Đã giao")
                       .Sum(o => o.TotalAmount);
        }

        public int GetTotalOrders() => _dal.GetAll().Count;

        public List<OrderDTO> GetRecentOrders(int top = 5)
        {
            return _dal.GetAll()
                       .OrderByDescending(o => o.OrderDate)
                       .Take(top)
                       .ToList();
        }
    }
}
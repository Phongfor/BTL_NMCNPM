using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Chờ xác nhận";
        public string ShipAddress { get; set; } = "";
        public string Note { get; set; } = "";

        // Danh sách chi tiết đơn hàng (dùng khi cần load kèm)
        public List<OrderDetailDTO> OrderDetails { get; set; } = new();
    }
}

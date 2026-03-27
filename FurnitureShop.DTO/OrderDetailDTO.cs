using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.DTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        // Thành tiền — tự tính
        public decimal SubTotal => Quantity * UnitPrice;
    }
}

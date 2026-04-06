using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.DTO
{
    public class CartItemDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        // Thành tiền — tự tính
        public decimal SubTotal => Price * Quantity;
    }
}
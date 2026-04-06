using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public decimal Price { get; set; }
        public int Discount { get; set; } = 0;
        // Giá sau khi giảm — tự tính, không lưu DB
        public decimal SalePrice => Discount > 0 ? Price * (1 - Discount / 100m) : Price;
        public int Stock { get; set; }
        public string Description { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }
}

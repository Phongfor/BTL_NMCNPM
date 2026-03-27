using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string Role { get; set; } = "Customer"; // "Admin" hoặc "Customer"
        public DateTime CreatedDate { get; set; }
    }
}

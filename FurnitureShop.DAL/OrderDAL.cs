using Microsoft.Data.SqlClient;
using System.Data;
using FurnitureShop.DTO;

namespace FurnitureShop.DAL
{
    public class OrderDAL
    {
        private readonly DBConnection _db;

        public OrderDAL(string connectionString)
        {
            _db = new DBConnection(connectionString);
        }

        // Tạo đơn hàng mới, trả về OrderID vừa tạo
        public int CreateOrder(OrderDTO order)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                // Bước 1: Insert vào Orders
                var cmdOrder = new SqlCommand("sp_CreateOrder", conn, transaction);
                cmdOrder.CommandType = CommandType.StoredProcedure;
                cmdOrder.Parameters.AddWithValue("@UserID", order.UserID);
                cmdOrder.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmdOrder.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                cmdOrder.Parameters.AddWithValue("@Note", order.Note ?? "");

                var newOrderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                // Bước 2: Insert từng OrderDetail
                foreach (var item in order.OrderDetails)
                {
                    var cmdDetail = new SqlCommand("sp_CreateOrderDetail", conn, transaction);
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.Parameters.AddWithValue("@OrderID", newOrderId);
                    cmdDetail.Parameters.AddWithValue("@ProductID", item.ProductID);
                    cmdDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmdDetail.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                    cmdDetail.ExecuteNonQuery();
                }

                transaction.Commit();
                return newOrderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // Lấy danh sách đơn hàng của 1 user
        public List<OrderDTO> GetByUser(int userId)
        {
            var list = new List<OrderDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetOrdersByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userId);
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OrderDTO
                {
                    OrderID = (int)dr["OrderID"],
                    UserID = (int)dr["UserID"],
                    OrderDate = (DateTime)dr["OrderDate"],
                    TotalAmount = (decimal)dr["TotalAmount"],
                    Status = dr["Status"].ToString()!,
                    ShipAddress = dr["ShipAddress"]?.ToString() ?? "",
                    Note = dr["Note"]?.ToString() ?? ""
                });
            }
            return list;
        }

        // Lấy toàn bộ đơn hàng (Admin)
        public List<OrderDTO> GetAll()
        {
            var list = new List<OrderDTO>();
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_GetAllOrders", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OrderDTO
                {
                    OrderID = (int)dr["OrderID"],
                    UserID = (int)dr["UserID"],
                    FullName = dr["FullName"]?.ToString() ?? "",
                    OrderDate = (DateTime)dr["OrderDate"],
                    TotalAmount = (decimal)dr["TotalAmount"],
                    Status = dr["Status"].ToString()!,
                    ShipAddress = dr["ShipAddress"]?.ToString() ?? ""
                });
            }
            return list;
        }

        // Lấy chi tiết 1 đơn hàng kèm danh sách sản phẩm
        public OrderDTO? GetByID(int orderId)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            // Lấy thông tin đơn hàng
            var cmdOrder = new SqlCommand("sp_GetOrderByID", conn);
            cmdOrder.CommandType = CommandType.StoredProcedure;
            cmdOrder.Parameters.AddWithValue("@OrderID", orderId);
            var drOrder = cmdOrder.ExecuteReader();

            if (!drOrder.Read()) return null;

            var order = new OrderDTO
            {
                OrderID = (int)drOrder["OrderID"],
                UserID = (int)drOrder["UserID"],
                FullName = drOrder["FullName"]?.ToString() ?? "",
                OrderDate = (DateTime)drOrder["OrderDate"],
                TotalAmount = (decimal)drOrder["TotalAmount"],
                Status = drOrder["Status"].ToString()!,
                ShipAddress = drOrder["ShipAddress"]?.ToString() ?? "",
                Note = drOrder["Note"]?.ToString() ?? ""
            };
            drOrder.Close();

            // Lấy chi tiết đơn hàng
            var cmdDetail = new SqlCommand("sp_GetOrderDetails", conn);
            cmdDetail.CommandType = CommandType.StoredProcedure;
            cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
            var drDetail = cmdDetail.ExecuteReader();
            while (drDetail.Read())
            {
                order.OrderDetails.Add(new OrderDetailDTO
                {
                    OrderDetailID = (int)drDetail["OrderDetailID"],
                    OrderID = orderId,
                    ProductID = (int)drDetail["ProductID"],
                    ProductName = drDetail["ProductName"]?.ToString() ?? "",
                    ImageURL = drDetail["ImageURL"]?.ToString() ?? "",
                    Quantity = (int)drDetail["Quantity"],
                    UnitPrice = (decimal)drDetail["UnitPrice"]
                });
            }
            return order;
        }

        // Admin cập nhật trạng thái đơn hàng
        public bool UpdateStatus(int orderId, string status)
        {
            using var conn = _db.GetConnection();
            var cmd = new SqlCommand("sp_UpdateOrderStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            cmd.Parameters.AddWithValue("@Status", status);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
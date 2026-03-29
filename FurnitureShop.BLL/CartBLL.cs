using FurnitureShop.DTO;

namespace FurnitureShop.BLL
{
    public class CartBLL
    {
        // Thêm sản phẩm vào giỏ
        public List<CartItemDTO> AddToCart(List<CartItemDTO> cart,
                                            ProductDTO product, int quantity)
        {
            if (product == null) return cart;
            if (quantity <= 0) return cart;

            var item = cart.FirstOrDefault(x => x.ProductID == product.ProductID);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItemDTO
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ImageURL = product.ImageURL,
                    Price = product.SalePrice,
                    Quantity = quantity
                });
            }
            return cart;
        }

        // Cập nhật số lượng
        public List<CartItemDTO> UpdateQuantity(List<CartItemDTO> cart,
                                                 int productId, int quantity)
        {
            var item = cart.FirstOrDefault(x => x.ProductID == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Remove(item);   // Số lượng = 0 → xóa luôn
                else
                    item.Quantity = quantity;
            }
            return cart;
        }

        // Xóa 1 sản phẩm khỏi giỏ
        public List<CartItemDTO> RemoveFromCart(List<CartItemDTO> cart, int productId)
        {
            var item = cart.FirstOrDefault(x => x.ProductID == productId);
            if (item != null) cart.Remove(item);
            return cart;
        }

        // Tổng tiền
        public decimal GetTotal(List<CartItemDTO> cart)
        {
            return cart.Sum(x => x.SubTotal);
        }

        // Tổng số lượng sản phẩm (dùng hiển thị icon giỏ hàng)
        public int GetTotalQuantity(List<CartItemDTO> cart)
        {
            return cart.Sum(x => x.Quantity);
        }

        // Xóa toàn bộ giỏ hàng (sau khi đặt hàng thành công)
        public List<CartItemDTO> ClearCart()
        {
            return new List<CartItemDTO>();
        }
    }
}
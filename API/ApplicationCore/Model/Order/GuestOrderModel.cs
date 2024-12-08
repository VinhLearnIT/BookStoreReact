
namespace ApplicationCore.Model.Order
{
    public class GuestOrderModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Cccd { get; set; }
        public string? Address { get; set; }
        public List<CartItem>? CartItems { get; set; } 
        public string? PaymentMethod { get; set; }
    }
    public class CartItem
    {
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

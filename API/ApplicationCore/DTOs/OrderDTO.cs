namespace ApplicationCore.DTOs
{
    public class OrderDTO
    {
        public int? OrderID { get; set; }
        public int? CustomerID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CCCD { get; set; }
        public string? Address { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentMethod { get; set; }

    }
}

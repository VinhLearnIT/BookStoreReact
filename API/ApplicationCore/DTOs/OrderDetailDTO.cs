namespace ApplicationCore.DTOs
{
    public class OrderDetailDTO
    {
        public int? OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public string? BookName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

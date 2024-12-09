namespace ApplicationCore.DTOs
{
    public class ShoppingCartDTO
    {
        public int? CartID { get; set; }
        public int? CustomerID { get; set; }
        public int BookID { get; set; }
        public string? BookName { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}

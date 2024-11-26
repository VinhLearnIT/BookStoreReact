namespace ApplicationCore.DTOs
{
    public class PaymentDTO
    {
        public int? PaymentID { get; set; }
        public int OrderID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
    }
}

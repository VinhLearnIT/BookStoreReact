using System.Text.Json.Serialization;

namespace ApplicationCore.DTOs
{
    public class CustomerDTO
    {
        public int? CustomerID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CCCD { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public bool? FullInfo { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

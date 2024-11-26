using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public DateTime PublishedDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }

        public string? Categories { get; set; }
    }
}

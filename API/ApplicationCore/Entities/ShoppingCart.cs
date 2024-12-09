using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }

        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        public int BookID { get; set; }
        public Book? Book { get; set; }

        public int Quantity { get; set; }
    }
}

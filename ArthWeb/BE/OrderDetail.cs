using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; }

        [Required]
        public string ItemKey { get; set; }

        [Required]
        public string ItemSize { get; set; }

        [Required]
        public int ItemQuantity { get; set; }

        [Required]
        public decimal PricePerItem { get; set; }

        [Required]
        public int OrderID { get; set; }
    }
}

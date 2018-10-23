using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int AddressID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime OrderDeliveryDate { get; set; }

        public DateTime OrderDeliveredDate { get; set; }

        [Required]
        public bool Delivered { get; set; }
    }
}

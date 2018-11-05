using System;
using System.Collections.Generic;
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
        public DateTime OrderDate { get; set; }

        public DateTime OrderDeliveryDate { get; set; }

        public DateTime OrderDeliveredDate { get; set; }

        [Required]
        public bool Delivered { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }

        [Required]
        [StringLength(10)]
        public String Phone { get; set; }

        [StringLength(10)]
        public String AltPhone { get; set; }

        [Required]
        [StringLength(10)]
        public String Pin { get; set; }

        [Required]
        [StringLength(200)]
        public String AddressDetail { get; set; }

        [Required]
        [StringLength(50)]
        public String City { get; set; }

        [Required]
        [StringLength(50)]
        public String State { get; set; }

        [StringLength(50)]
        public String Landmark { get; set; }

        [Required]
        [StringLength(10)]
        public String Type { get; set; }

        [NotMapped]
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

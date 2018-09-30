using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public int AddressID { get; set; }

        [Required]
        public int UserID { get; set; }

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
    }
}

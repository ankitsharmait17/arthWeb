using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("User")]
    public class User
    {
        public int UserID { get; set; }

        [StringLength(254)]
        [Key]
        public String EmailID { get; set; }

        [StringLength(200)]
        public String Password { get; set; }

        [StringLength(50)]
        public String Name { get; set; }

        [StringLength(10)]
        public String Phone { get; set; }

        public bool IsActive { get; set; }
    }
}

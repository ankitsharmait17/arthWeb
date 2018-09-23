using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("User")]
    public class User
    {
        [StringLength(254)]
        [Key]
        public String EmailID { get; set; }

        [StringLength(200)]
        public String Password { get; set; }

        [StringLength(50)]
        public String FirstName { get; set; }

        [StringLength(50)]
        public String LastName { get; set; }

        [StringLength(10)]
        public String Phone { get; set; }

        public bool IsActive { get; set; }
    }
}

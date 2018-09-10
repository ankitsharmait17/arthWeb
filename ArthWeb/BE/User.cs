using System;
using System.ComponentModel.DataAnnotations;

namespace BE
{
    public class User
    {
        [StringLength(254)]
        [Key]
        public String EmailID { get; set; }

        [StringLength(50)]
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

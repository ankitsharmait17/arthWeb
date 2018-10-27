using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArthWeb.Models
{
    public class LoginModel
    {
        [DisplayName("EmailID")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        public string Password { get; set; }
        
    }
}
using System.Collections.Generic;

namespace BE.Models
{
    public class UserAddressModel
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public List<Address> Addresses { get; set; }

        public List<OrderModel> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public string Username { get; set; }

        public Address Address { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime OrderDeliveryDate { get; set; }

        public DateTime OrderDeliveredDate { get; set; }

        public bool Delivered { get; set; }

        public string Status { get; set; }

        public List<OrderDetailModel> OrderDetails { get; set; }
    }
}

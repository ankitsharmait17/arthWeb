using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }

        public string ItemKey { get; set; }

        public string ItemDesc { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerItem { get; set; }

        public string Size { get; set; }
    }
}

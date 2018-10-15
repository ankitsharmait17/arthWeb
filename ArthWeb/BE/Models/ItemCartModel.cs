using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class ItemCartModel
    {
        public string ItemKey { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }
    }
}

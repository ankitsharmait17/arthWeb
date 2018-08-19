using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class ItemModel
    {
        public int ItemID { get; set; }

        public string ItemKey { get; set; }

        public string Description { get; set; }

        public string DescriptionLong { get; set; }

        public string ItemType { get; set; }

        public string ItemSubType { get; set; }

        public string Gender { get; set; }

        public decimal Price { get; set; }
    }
}

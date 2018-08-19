using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ItemMappingBL
    {
        public Dictionary<string,List<string>> GetItemMappingDict()
        {
            return new ItemMappingDAO().GetItemMappingsDict();
        }
    }
}

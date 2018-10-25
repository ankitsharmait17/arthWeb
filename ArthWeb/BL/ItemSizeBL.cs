using BE;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ItemSizeBL
    {
        public List<ItemSize> GetItemSizesfromItemKey(string itemKey)
        {
            return new ItemSizeDAO().GetItemSizesfromItemKey(itemKey);
        }

        public List<string> GetAllSizes()
        {
            return new ItemSizeDAO().GetAllSizes();
        }
    }
}

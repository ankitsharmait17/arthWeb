using BE;
using BE.Models;
using DL;
using System.Collections.Generic;

namespace BL
{
    public class ItemBL
    {
        public List<ItemModel> GetItemsforGrid(string search, int pageSize, int startRec, string order, string filterGender, string filterSubtype, string filterPrice,string filterSize)
        {
            return new ItemDAO().GetItemsforGrid(search, pageSize, startRec, order, filterGender, filterSubtype,  filterPrice, filterSize);
        }

        public Item GetItem(string itemKey)
        {
            return new ItemDAO().GetItem(itemKey);
        }

        public List<ItemCartModel> GetCartItems(List<ItemCartModel> items,bool sizeCheck)
        {
            return new ItemDAO().GetCartItems(items,sizeCheck);
        }
    }
}

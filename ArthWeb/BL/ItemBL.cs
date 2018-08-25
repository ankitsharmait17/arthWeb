using BE.Models;
using DL;
using System.Collections.Generic;

namespace BL
{
    public class ItemBL
    {
        public List<ItemModel> GetItemsforGrid(string search, int pageSize, int startRec, string order)
        {
            return new ItemDAO().GetItemsforGrid(search, pageSize, startRec, order);
        }
    }
}

using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class ItemSizeDAO
    {
        public List<ItemSize> GetItemSizesfromItemKey(string itemKey)
        {
            List<ItemSize> itemSizes = null;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                     itemSizes = (from item in cntx.Items where item.ItemKey.Equals(itemKey)
                                join qty in cntx.ItemQuantities
                                on item.ItemID equals qty.ItemID where qty.Quantity>0
                                join size in cntx.ItemSizes
                                on qty.ItemSizeID equals size.ItemSizeID
                                select new { size }).AsEnumerable()
                                .Select(x => new ItemSize()
                                {
                                    ItemSizeID = x.size.ItemSizeID,
                                    ItemSizeName = x.size.ItemSizeName
                                }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return itemSizes;
        }
    }
}

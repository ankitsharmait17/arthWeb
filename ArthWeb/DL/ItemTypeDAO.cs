using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class ItemTypeDAO
    {
        public List<ItemType> GetItemType()
        {
            List<ItemType> typelist = null;
            using (ArthModel cntx= new ArthModel())
            {
                typelist = cntx.ItemTypes.ToList();
            }
            return typelist;
        }
    }
}

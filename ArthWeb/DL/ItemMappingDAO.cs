using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class ItemMappingDAO
    {
        public List<ItemMapping> GetItemMappings()
        {
            List<ItemMapping> itemMappings = null;
            using(ArthModel cntx=new ArthModel())
            {
                itemMappings = cntx.ItemMappings.ToList();
            }
            return itemMappings;
        }

        public Dictionary<string,List<string>> GetItemMappingsDict()
        {
            Dictionary<string, List<string>> pairs = null;
            using(ArthModel cntx=new ArthModel())
            {
                var data = (from map in cntx.ItemMappings
                            join type in cntx.ItemTypes
                            on map.ItemTypeID equals type.ItemTypeID
                            join subtype in cntx.ItemSubTypes
                            on map.ItemSubTypeID equals subtype.ItemSubTypeID
                            select new { map, type, subtype }).GroupBy(x => x.type.ItemTypeKey)
                            .AsEnumerable();
                pairs = new Dictionary<string, List<string>>();
                foreach(var it in data)
                {
                    pairs.Add(it.Key, it.Select(x => x.subtype.ItemSubTypeKey).ToList());
                }
            }
            return pairs;
        }
    }
}

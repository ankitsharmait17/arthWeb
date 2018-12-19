using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class MenuDAO
    {
        public Dictionary<string ,List<string>> GetMenuItems(string gender)
        {
            Dictionary<string, List<string>> pairs = null;
            using (ArthModel cntx=new ArthModel())
            {
                var data = cntx.Menus.Where(x=>x.Gender.Equals(gender)).GroupBy(x => x.ItemType).AsEnumerable();
                pairs = new Dictionary<string, List<string>>();
                foreach (var it in data)
                {
                    pairs.Add(it.Key, it.Select(x=>x.ItemSubType).ToList());
                }
            }
            return pairs;
        }
    }
}

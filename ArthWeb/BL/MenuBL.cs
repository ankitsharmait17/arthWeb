using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MenuBL
    {
        public Dictionary<string, List<string>> GetMenuItems(string gender)
        {
            return new MenuDAO().GetMenuItems(gender);
        }
    }
}

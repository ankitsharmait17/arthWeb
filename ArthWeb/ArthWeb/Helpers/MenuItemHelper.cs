﻿using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArthWeb.Helpers
{
    public class MenuItemHelper
    {
        public static Dictionary<string, List<string>> GetMenuItems(string gender)
        {
            return new MenuBL().GetMenuItems(gender);
        }
    }
}
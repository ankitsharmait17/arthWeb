using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArthWeb.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index(string itemKey)
        {
            Item item = null;
            try
            {
                item = new ItemBL().GetItem(itemKey);
            }
            catch (Exception ex)
            {
                RedirectToAction("Index", "Home");
            }
            return View(item);
        }
    }
}
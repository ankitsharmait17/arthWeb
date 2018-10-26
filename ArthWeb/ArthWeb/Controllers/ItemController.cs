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
        public ActionResult ViewItem(string itemKey)
        {
            Item item = null;
            try
            {
                item = new ItemBL().GetItem(itemKey);
                var size = new ItemSizeBL().GetItemSizesfromItemKey(itemKey);
                ViewBag.sizes = size.Select(x => new SelectListItem { Text = x.ItemSizeName, Value = x.ItemSizeID.ToString() });
            }
            catch (Exception ex)
            {
                RedirectToAction("Index", "Home");
            }
            return View(item);
        }
    }
}
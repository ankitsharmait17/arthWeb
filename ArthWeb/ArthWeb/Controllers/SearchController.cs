using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArthWeb.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            try
            {
                ViewBag.DDList = new ItemMappingBL().GetItemMappingDict();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public ActionResult GetItemJsonData(string search, int pageSize, int startRec, string order, string orderDir)
        {
            try
            {
                var itemList = new ItemBL().GetItemsforGrid(search, pageSize, startRec, order, orderDir);
                return Json(new {Success=true, data=itemList });
            }
            catch (Exception ex)
            {
                return Json(new { Success = true, Message="Sorry for the inconvenience." });
            }
        }
    }
}
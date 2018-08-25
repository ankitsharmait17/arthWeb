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
        public ActionResult Index(string search=null)
        {
            try
            {
                ViewBag.DDList = new ItemMappingBL().GetItemMappingDict();
                ViewBag.searchString = search;
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        
        [HttpGet]
        public ActionResult GetItemJsonData(string search, int pageSize, int startRec, string order)
        {
            try
            {
                var itemList = new ItemBL().GetItemsforGrid(search, pageSize, startRec, order);
                return Json(new {Success=true, data=itemList },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = true, Message="Sorry for the inconvenience." });
            }
        }
    }
}
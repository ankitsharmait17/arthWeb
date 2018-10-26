using ArthWeb.Filters;
using BE;
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
        public ActionResult SearchItems(string search=null)
        {
            try
            {
                ViewBag.DDList = new ItemMappingBL().GetItemMappingDict();
                ViewBag.sizes = new ItemSizeBL().GetAllSizes();
                ViewBag.searchString = string.IsNullOrWhiteSpace(search)?"":search;
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        
        [HttpGet]
        public ActionResult GetItemJsonData(string search, int pageSize, int startRec, string order, string filterGender, string filterSubtype, string filterPrice,string filterSize)
        {
            try
            {
                var itemList = new ItemBL().GetItemsforGrid(search, pageSize, startRec, order, filterGender, filterSubtype, filterPrice,filterSize);
                if(itemList==null || itemList.Count()==0)
                    return Json(new { Success = false, Message = "No items found." },JsonRequestBehavior.AllowGet);
                else
                    return Json(new {Success=true, data=itemList },JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false, Message="Sorry for the inconvenience. Please reload the page and try again." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetItemSizes(string itemKey)
        {
            List<ItemSize> itemSizes = null;
            try
            {
                itemSizes = new ItemSizeBL().GetItemSizesfromItemKey(itemKey);
                return Json(new { Success = true, data = itemSizes },JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = true,Message="Try adding again." });
            }
        }
    }
}
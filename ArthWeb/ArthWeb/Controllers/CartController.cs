using BE.Models;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArthWeb.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult CartItems()
        {
            List<ItemCartModel> items = null;
            try
            {
                if (Session["cart"] == null)
                    return View(items);
                List<ItemCartModel> li = (List<ItemCartModel>)Session["cart"];
                items =new ItemBL().GetCartItems(li);
            }
            catch (Exception)
            {

                throw;
            }
            return View(items);
        }

        [HttpPost]
        public ActionResult Add(ItemCartModel item)
        {
            int count = 0;
            try
            {
                if (Session["cart"] == null)
                {
                    List<ItemCartModel> li = new List<ItemCartModel>();
                    li.Add(item);
                    Session["cart"] = li;
                    ViewBag.cart = li.Count();
                    count = 1;
                    Session["count"] = count;
                }
                else
                {
                    List<ItemCartModel> li = (List<ItemCartModel>)Session["cart"];
                    var check=li.FirstOrDefault(x => x.ItemKey.Equals(item.ItemKey));
                    if (check != null)
                        check.Quantity += item.Quantity;
                    else
                        li.Add(item);
                    Session["cart"] = li;
                    ViewBag.cart = li.Count();
                    count = Convert.ToInt32(Session["count"]) + 1;
                    Session["count"] = count;
                }
                return Json(new { Success = true,data=count });
            }
            catch (Exception)
            {
                return Json(new { Success = false });
            }
        }
    }
}
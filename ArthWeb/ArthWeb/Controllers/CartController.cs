using ArthWeb.Filters;
using BE.Models;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArthWeb.Controllers
{
    
    public class CartController : Controller
    {
        // GET: Cart
        [Authorize]
        public ActionResult CartItems()
        {
            List<ItemCartModel> items = null;
            try
            {
                if (Session["cart"] == null)
                    return RedirectToAction("Index","Home");
                List<ItemCartModel> li = (List<ItemCartModel>)Session["cart"];
                if (!li.Any())
                {
                    Session["cart"] = null;
                    Session["count"] = 0;
                    return RedirectToAction("Index", "Home");
                }
                items =new ItemBL().GetCartItems(li,true);
                if (items.Count != li.Count)
                {
                    ViewBag.Error = "Some items had to be removed from your cart because they were low in stock.";
                    Session["cart"] = items;
                }
                var id = User.Identity.Name;
                var address = new AddressBL().GetAddressforUserID(new UserBL().GetUser(id).UserID);
                if (address != null)
                {
                    ViewBag.addresses = address.Select(x => new SelectListItem()
                    {
                        Text = x.Type + " (" + x.AddressDetail + ", " + x.City + ", " + x.State + ", " + x.Pin+") "+" Phone: "+x.Phone,
                        Value = x.AddressID.ToString()
                    });
                }
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
                    var check=li.FirstOrDefault(x => x.ItemKey.Equals(item.ItemKey) && x.Size.Equals(item.Size));
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

        [Authorize]
        [HttpPost]
        public ActionResult Remove(string itemKey)
        {
            try
            {
                List<ItemCartModel> li = (List<ItemCartModel>)Session["cart"];
                var check = li.RemoveAll(x => x.ItemKey.Equals(itemKey));
                Session["count"] = li.Count;
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult PlaceOrder(int id)
        {
            try
            {
                var usercheck = new UserBL().GetUser(User.Identity.Name);
                if (usercheck.IsActive == false)
                {
                    return Json(new { Success = false, Message = "Your account is not activated. Please activate your account to place the order. The link was sent to you at the time of signing up." });
                }
                else
                {
                    List<ItemCartModel> li = (List<ItemCartModel>)Session["cart"];
                    var exList = new OrderBL().PlaceOrder(id, User.Identity.Name, li);
                    if (exList.Count() == 1)
                    {
                        Session["cart"] = null;
                        Session["count"] = 0;
                        Task.Run(() => SendOrderSuccessMail(User.Identity.Name,li));
                        return Json(new { Success = true, Message = "Order Placed", data = exList });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = exList });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        public void SendOrderSuccessMail(string email,List<ItemCartModel> li)
        {
            try
            {
                decimal price = 0;
                var items = new ItemBL().GetCartItems(li,false);
                string name = new UserBL().GetUser(email).Name;
                string body = "Hello " + name + ",";
                body += "<br/><br/>Your order has been placed. You can check the order status from your profile.";
                body += "<br /><br />Order Details:<br/>";
                body += "<table border="+ 1 + " cellpadding=" + 1 + " cellspacing=" + 1 + " width = " + 400 + ">";
                body += "<tr bgcolor='#4da6ff'><th>Items</th><th>Size</th><th>Price</th>";
                foreach(var item in items)
                {
                    body += "<tr>";
                    body += "<td>" + item.Description + "</td>";
                    body += "<td>" + item.Size + "</td>";
                    body += "<td>" + item.Quantity +" X  Rs." + item.Price +"</td>";
                    body += "</tr>";
                    price += item.Price * item.Quantity;
                }
                body += "<tr><td></td><td>Total:</td><td>Rs." + price + "</td></tr>";
                body += "</table>";
                body += "<br/><br/>We will keep you informed about the order status.";
                body += "<br/><br/>Thanks";
                string subject = "Arth support : Order placed.";
                new MailHelperBL().SendEmail(email, name, body, subject);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
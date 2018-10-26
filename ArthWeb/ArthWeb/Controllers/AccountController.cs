using ArthWeb.Filters;
using ArthWeb.Helpers;
using ArthWeb.Models;
using BE;
using BE.Models;
using BL;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ArthWeb.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Login(string ReturnUrl = "")
        {
            string retUrl = null;
            try
            {
                if (string.IsNullOrWhiteSpace(ReturnUrl))
                    retUrl = Url.Action("Index", "Home");
                else
                    retUrl = ReturnUrl;
                ViewBag.ReturnUrl = retUrl;
                if (Request.IsAuthenticated)
                {
                    return Redirect(retUrl);
                }
                var expiredAuthProcessCookie =new Ticket().DestroyCookie(
                    ConfigurationManager.AppSettings["AUTH_PROCESS_COOKIE"],Request.Url.Host);
                Response.Cookies.Add(expiredAuthProcessCookie);
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleAntiForgeryErrorOnLogin]
        public ActionResult Login(LoginModel data,string ReturnUrl)
        {
            string retUrl = null;
            try
            {
                if (string.IsNullOrWhiteSpace(ReturnUrl))
                    retUrl = Url.Action("Index", "Home");
                else
                    retUrl = ReturnUrl;
                if (Request.IsAuthenticated)
                {
                    return Redirect(retUrl);
                }
                var authProcessCookie = new Ticket().CreateCookie(
                    ConfigurationManager.AppSettings["AUTHENTICATION_PROCESS_COOKIE"],
                    Cryptography.Protect(data.UserName, "AUTHENTICATION_PROCESS"),
                    DateTime.Now.AddSeconds(Convert.ToInt32(3600)),
                    Request.Url.Host
                    );
                Response.Cookies.Add(authProcessCookie);
                var isUsrAuthenticated = new UserBL().ValidateUser(data.UserName, data.Password);
                if (isUsrAuthenticated)
                {
                    var formscookie = new Ticket().CreateAuthenticationCookie(data.UserName);
                    Response.Cookies.Add(formscookie);
                    //var authCookieValue = Request.Headers.Get("authCookieValue");
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(formscookie.Value);
                    var userData = JsonConvert.DeserializeObject<UserData>(authTicket.UserData);
                    var identity = new GenericIdentity(authTicket.Name, "Forms");
                    var principal = new GenericPrincipal(identity, null);
                    HttpContext.User = principal;
                    return Redirect(retUrl);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", routeValues: new { ReturnUrl = retUrl });
            }
            return RedirectToAction("Login", new { ReturnUrl = retUrl });
        }

        [CustomAuthorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Session.Clear();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            var formsCookie = new Ticket().DestroyAuthenticationCookie();
            Response.Cookies.Add(formsCookie);
            Response.Cookies["AUTHCOOKIE"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index","Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(User user)
        {
            try
            {
                string url = Request.Url.AbsoluteUri.Replace("RegisterUser", "ConfirmEmail");
                var isSuccess = new UserBL().AddUser(user,url);
                return Json(new { Success = isSuccess });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustomAuthorize]
        public ActionResult Profile()
        {
            var username = User.Identity.Name;
            UserAddressModel user = null;
            try
            {
                user = new UserBL().GetUserWithAdresses(username);
            }
            catch (Exception)
            {
                RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public ActionResult UpdateUser(User user)
        {
            try
            {
                bool isSuccess = new UserBL().UpdateUser(user);
                if(isSuccess)
                    return Json(new { Success = isSuccess, Message = "Your details have been updated." });
                else
                    return Json(new { Success = isSuccess, Message = "Your details could not be updated." });
            }
            catch (Exception)
            {
                return Json(new { Success = false, Message = "Sorry,your details have not been updated." });
            }
        }

        public ActionResult AddAddress(Address address)
        {
            try
            {
                address.UserID = new UserBL().GetUser(User.Identity.Name).UserID;
                int id = new AddressBL().AddAddress(address);
                bool isSuccess = id == 0 ? false : true;
                if(isSuccess)
                    return Json(new { Success =isSuccess , Message = "Address added.",data=id });
                else
                    return Json(new { Success = isSuccess, Message = "Address couldn't be added."});
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = "Address could not be added." });
            }
        }

        public ActionResult UpdateAddress(Address address)
        {
            try
            {
                address.UserID = new UserBL().GetUser(User.Identity.Name).UserID;
                bool isSuccess = new AddressBL().UpdateAddress(address);
                if(isSuccess)
                    return Json(new { Success = isSuccess, Message = "Saved changes."});
                else
                    return Json(new { Success = isSuccess, Message = "Could not save changes." });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = "Address could not be updated." });
            }
        }

        public ActionResult DeleteAddress(int id)
        {
            try
            {
                bool isSuccess = new AddressBL().DeleteAddress(id);
                if (isSuccess)
                    return Json(new { Success = isSuccess, Message = "Address deleted." });
                else
                    return Json(new { Success = isSuccess, Message = "Could not delete address." });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = "Address could not be deleted." });
            }
        }

        public ActionResult ConfirmEmail(string email,string code)
        {
            try
            {
                var isSuccess = new UserBL().ConfirmEmail(email, code);
                ViewBag.MessageFix = "Redirecting now...";
                if (isSuccess == 1)
                    ViewBag.Message = "Some problem with verification of email.Please try again.";
                else if(isSuccess==2)
                    ViewBag.Message = "Verification link expired.";
                else
                    ViewBag.Message = "Verified successfully";

            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword,string newPassword)
        {
            try
            {
                var username = User.Identity.Name;
                var isSuccess = new UserBL().ChangePassword(username, oldPassword, newPassword);
                if (isSuccess == 0)
                    return Json(new { Success = true, Message = "Password changed." });
                else if (isSuccess == 1)
                    return Json(new { Success = false, Message = "Email or Password entered does not match." });
                else
                    return Json(new { Success = false, Message = "Couldn't change password.Please try again." });

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ViewOrder(int orderID)
        {
            OrderModel order = null;
            try
            {
                int userID = new UserBL().GetUser(User.Identity.Name).UserID;
                order = new OrderBL().GetOrder(orderID,userID);
                if (order == null)
                {
                    return RedirectToAction("Profile", "Account");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Profile", "Account");
            }
            return View(order);
        }

    }
}
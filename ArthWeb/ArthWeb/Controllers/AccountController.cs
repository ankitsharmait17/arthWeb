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
                var isSuccess = new UserBL().AddUser(user);
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
            User user = null;
            try
            {
                user = new UserBL().GetUser(username);
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
                return Json(new { Success = true, Message = "Your details have been updated." });
            }
            catch (Exception)
            {
                return Json(new { Success = false, Message = "Sorry,Your details have not been updated." });
            }
        }

    }
}
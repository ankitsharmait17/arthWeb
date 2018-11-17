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
using System.Threading.Tasks;
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
                var isUsrAuthenticated = new UserBL().ValidateUser(data.UserName, data.Password);
                if (isUsrAuthenticated)
                {
                    var formscookie = new Ticket().CreateAuthenticationCookie(data.UserName,data.RememberMe);
                    Response.Cookies.Add(formscookie);
                    var identity = new GenericIdentity(FormsAuthentication.FormsCookieName, "Forms");
                    var principal = new GenericPrincipal(identity, null);
                    HttpContext.User = principal;
                    return Redirect(retUrl);
                }
                else
                {
                    throw new Exception("Email Id or password is incorrect.Please try again.");
                }
            }
            catch (Exception ex)
            {
                //TempData["Fail"] = "Email Id or password is incorrect.Please try again.";
                //return RedirectToAction("Index", "Common", new { retUrl = Url.Action("Login", "Account") });
                throw;
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
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
                string url = Request.Url.AbsoluteUri.Replace("RegisterUser", "ConfirmEmail");
                var code = new UserBL().AddUser(user);
                if (code != null)
                {
                    Task.Run(() => SendConfirmationMail(url, user.EmailID, code));
                }
                return Json(new { Success= (code==null)? false:true });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        public ActionResult ForgotPassword()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            string code = null;
            try
            {
                code= new UserBL().ForgotPasswordEnable(forgotPassword.Email);
                if (code!= null)
                {
                    TempData["Success"] = "An email has been sent you with the link to change your password.";
                    string url = Request.Url.AbsoluteUri.Replace("ForgotPassword", "ResetPassword");
                    Task.Run(() => SendResetPasswordMail(url, forgotPassword.Email, code));
                }
                else
                {
                    TempData["Fail"] = "Some issue occured. Please try again.";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index", "Common", new { retUrl = Url.Action("Index", "Home") });
        }

        public ActionResult ResetPassword(string email, string code)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var check = new UserBL().ConfirmForgotPassword(code, email);
                if (check == false)
                {
                    TempData["Fail"] = "Link has expired.";
                    return RedirectToAction("Index", "Common", new { retUrl = Url.Action("Index", "Home") });
                }
                ViewBag.email = email;
            }
            catch(Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            try
            {
                var isSuccess = new UserBL().ResetPassword(reset.Email, reset.Password);
                if (isSuccess)
                {
                    TempData["Success"] = "Password reset success. Redirecting to login.";
                    Task.Run(() => SendResetPasswordSuccessMail(reset.Email));
                }
                else
                {
                    TempData["Fail"] = "Password reset failure. Please try again.";
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Common", new { retUrl = Url.Action("Login", "Account") });
        }

        public void SendResetPasswordMail(string url,string email,string actCode)
        {
            try
            {
                string name = new UserBL().GetUser(email).Name;
                url += "?email=" + email + "&code=" + actCode;
                string body = "Hello " + name + ",";
                body += "<br /><br />Please click the following link to reset your account password";
                body += "<br /><a href = '" + url + "'>Click here to reset your account password.</a>";
                body += "<br /><br />Thanks";
                string subject = "Arth support : Reset Password";
                new MailHelperBL().SendEmail(email, name, body, subject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendResetPasswordSuccessMail(string email)
        {
            try
            {
                string name = new UserBL().GetUser(email).Name;
                string body = "Hello " + name + ",";
                body += "<br /><br />Your account password has been reset. You can now login using your new password.";
                body += "<br /><br />Thanks";
                string subject = "Arth support : Reset Password Success.";
                new MailHelperBL().SendEmail(email, name, body, subject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendConfirmationMail(string url, string email, string actCode)
        {
            try
            {
                string name = new UserBL().GetUser(email).Name;
                url += "?email=" + email + "&code=" + actCode;
                string body = "Hello " + name + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + url + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";
                string subject = "Arth support :Please activate your profile";
                new MailHelperBL().SendEmail(email, name, body, subject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            try
            {
                int userID = new UserBL().GetUser(User.Identity.Name).UserID;
                var status = new OrderBL().ChangeOrderStatus(id, userID,"Cancelled");
                if (status)
                    return Json(new { Success = true, Message = "Order has been cancelled." });
                else
                    return Json(new { Success = false, Message = "Order could not be cancelled." });
            }
            catch (Exception)
            {
                return RedirectToAction("Profile", "Account");
            }
        }

        [HttpPost]
        public ActionResult ReturnOrder(int id)
        {
            try
            {
                int userID = new UserBL().GetUser(User.Identity.Name).UserID;
                var status = new OrderBL().ChangeOrderStatus(id, userID, "Return Initiated");
                if (status)
                    return Json(new { Success = true, Message = "Return has been initiated." });
                else
                    return Json(new { Success = false, Message = "Order cannot be returned." });
            }
            catch (Exception)
            {
                return RedirectToAction("Profile", "Account");
            }
        }
    }
}
using BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ArthWeb.Helpers
{
    public class Ticket
    {
        public HttpCookie CreateAuthenticationCookie(string username)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                Convert.ToInt32(ConfigurationManager.AppSettings["FORMS_AUTHENTICATION_TICKET_VERSION"]),                                     // ticket version
                username,                     // authenticated username
                DateTime.Now,                          // issueDate
                DateTime.Now.AddSeconds(Convert.ToInt64(3600)),           // expiryDate
                false,                        // true to persist across browser sessions
                new UserBL().GetUserData(username),                              // can be used to store additional user data such as roles(NOT TO BE DONE IN OUR CASE)
                FormsAuthentication.FormsCookiePath);  // the path for the cookie
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            formsCookie.HttpOnly = true;
            formsCookie.Secure = FormsAuthentication.RequireSSL;
            formsCookie.Domain = FormsAuthentication.CookieDomain;

            return formsCookie;

        }
        public HttpCookie DestroyAuthenticationCookie()
        {
            HttpCookie formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty);
            formsCookie.Expires = DateTime.Now.AddYears(-1);
            formsCookie.HttpOnly = true;
            formsCookie.Secure = FormsAuthentication.RequireSSL;
            formsCookie.Domain = FormsAuthentication.CookieDomain;

            return formsCookie;
        }
        public HttpCookie CreateCookie(string cookieName, string cookieValue, DateTime expirationTime, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = expirationTime;
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = domain;

            return cookie;

        }
        public HttpCookie DestroyCookie(string cookieName, string domain)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddYears(-1);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = domain;

            return cookie;
        }
    }
}
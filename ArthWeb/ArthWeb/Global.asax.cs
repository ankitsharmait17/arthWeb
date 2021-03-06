﻿using BE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ArthWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //    HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
        //    RouteData rd = RouteTable.Routes.GetRouteData(context);

        //    var requestAuthenticated = false;

        //    Dictionary<string, List<string>> locations = new Dictionary<string, List<string>>();

        //    locations.Add("Account", new List<string> { "LogOut" });

        //    foreach (var myLocation in locations)
        //    {
        //        //if (((HttpApplication)sender).Context.Request.Path.Contains(myLocation.Key))
        //        //{
        //        //    requestAuthenticated = false;
        //        //    return;
        //        //}
        //        if (rd.Values["controller"].Equals(myLocation.Key))
        //        {
        //            var equals = myLocation.Value.Where(x => x.Equals(rd.Values["action"])).FirstOrDefault();
        //            if (equals != null)
        //            {
        //                requestAuthenticated = false;
        //                return;
        //            }
        //        }
        //    }

        //    if (!requestAuthenticated)
        //    {
        //        HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //        var authCookieValue = Request.Headers.Get("authCookieValue");

        //        if (authCookie != null || authCookieValue != null)
        //        {
        //            authCookieValue = authCookieValue == null ? authCookie.Value : authCookieValue;

        //            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookieValue);

        //            if (authTicket.Version != Convert.ToInt32(ConfigurationManager.AppSettings["FORMS_AUTHENTICATION_TICKET_VERSION"]))
        //            {
        //                FormsAuthentication.SignOut();
        //            }

        //            else if (authTicket != null && !authTicket.Expired)
        //            {
        //                FormsAuthenticationTicket newAuthTicket = authTicket;
        //                requestAuthenticated = true;

        //                if (authCookie != null)
        //                {
        //                    newAuthTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
        //                    if (newAuthTicket != authTicket)
        //                    {
        //                        string encryptedTicket = FormsAuthentication.Encrypt(newAuthTicket);
        //                        // Add the cookie to the request to save it
        //                        HttpCookie newAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //                        newAuthCookie.HttpOnly = true;
        //                        newAuthCookie.Secure = FormsAuthentication.RequireSSL;
        //                        newAuthCookie.Domain = FormsAuthentication.CookieDomain;
        //                        Response.Cookies.Set(newAuthCookie);
        //                    }

        //                }
        //                // Set the context user.

        //                var userData = JsonConvert.DeserializeObject<UserData>(authTicket.UserData);
        //                var identity = new GenericIdentity(authTicket.Name, "Forms");
        //                var principal = new GenericPrincipal(identity, null);
        //                Context.User = principal;
        //            }

        //        }
        //    }

        //    if (!requestAuthenticated)
        //    {
        //        var requestUrl = Request.Url.AbsoluteUri;
        //        Context.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

        //        Response.Clear();
        //        if (!requestUrl.Contains(FormsAuthentication.LoginUrl))
        //            Response.RedirectPermanent(string.Format("{0}?returnUrl={1}", FormsAuthentication.LoginUrl, requestUrl));
        //    }


        //}
    }
}

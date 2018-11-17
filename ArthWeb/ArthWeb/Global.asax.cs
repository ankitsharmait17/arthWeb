using BE.Models;
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

        protected void Application_AuthenticateRequest(object sender,EventArgs e)
        {
            var requestAuthenticated = false;

            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationLocationCollection mylocationcollection = config.Locations;
            var path = ((System.Web.HttpApplication)sender).Context.Request.Path;
            foreach (ConfigurationLocation myLocation in mylocationcollection)
            {
                if (path.Contains(myLocation.Path) || path.Equals("/"))
                {
                    requestAuthenticated = true;
                    return;
                }
            }

            if (!requestAuthenticated)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket.Version != Convert.ToInt32(ConfigurationManager.AppSettings["FORMS_AUTHENTICATION_TICKET_VERSION"]))
                    {
                        FormsAuthentication.SignOut();
                    }
                    else if(authTicket!=null && !authTicket.Expired)
                    {
                        FormsAuthenticationTicket newAuthTicket = authTicket;
                        requestAuthenticated = true;
                        if (authCookie != null)
                        {
                            newAuthTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
                            if (newAuthTicket != authTicket)
                            {
                                string encryptedTicket = FormsAuthentication.Encrypt(newAuthTicket);
                                HttpCookie newAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                                newAuthCookie.HttpOnly = true;
                                newAuthCookie.Secure = FormsAuthentication.RequireSSL;
                                newAuthCookie.Domain = FormsAuthentication.CookieDomain;
                                Response.Cookies.Set(newAuthCookie);
                            }
                        }

                        var identity = new GenericIdentity(authTicket.Name, "Forms");
                        var principal = new GenericPrincipal(identity, null);
                        Context.User = principal;
                    }
                }

            }

            if (!requestAuthenticated)
            {
                var requestUrl = Request.Url.AbsoluteUri;
                Context.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                Response.Clear();
                if (!requestUrl.Contains(FormsAuthentication.LoginUrl))
                {
                    Response.RedirectPermanent(string.Format("{0}?returnUrl={1}", FormsAuthentication.LoginUrl, requestUrl));
                }
            }
        }
    }
}

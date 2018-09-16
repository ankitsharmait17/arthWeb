using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArthWeb.Filters
{
    public class HandleAntiForgeryErrorOnLogin: ActionFilterAttribute, IExceptionFilter
    {
        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {

            var exception = filterContext.Exception as HttpAntiForgeryException;
            if (exception != null)
            {

                try
                {
                    

                    var loggedInUserName = filterContext.HttpContext.User.Identity.Name;
                    var detectedUsernameForPost = filterContext.HttpContext.Request.Params.GetValues("UserName")[0];
                    

                    if (detectedUsernameForPost.Equals(loggedInUserName))
                    {
                        var routeValues = new RouteValueDictionary();
                        routeValues["controller"] = "Authentication";
                        routeValues["action"] = "Logout";
                        routeValues["area"] = "Common";
                        filterContext.Result = new RedirectToRouteResult(routeValues);
                        filterContext.ExceptionHandled = true;
                    }
                    else
                    {
                        throw exception;
                    }
                }
                catch (Exception)
                {

                    throw exception;
                }

            }
        }

        #endregion
    }
}

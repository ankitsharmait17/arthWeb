using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArthWeb.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index(string retUrl)
        {
            ViewBag.returnUrl = retUrl;
            return View();
        }
    }
}
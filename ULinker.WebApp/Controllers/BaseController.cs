using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ULinker.WebApp.Filters;

namespace ULinker.WebApp.Controllers
{
    [LoginFilter]
    public class BaseController : Controller
    {
        // GET: Base
        /*public ActionResult Index()
        {
            return View();
        }*/
    }
}
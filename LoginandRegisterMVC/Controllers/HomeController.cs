using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Web.Mvc;

namespace LoginandRegisterMVC.Controllers
{
    public class HomeController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            log.Info("In Home Page");
            return View();
        }


        public ActionResult About()
        {
            log.Info("In About Page");
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            log.Info("In Contact Page");
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
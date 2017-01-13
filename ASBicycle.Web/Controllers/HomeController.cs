using System;
using System.Web.Mvc;

namespace ASBicycle.Web.Controllers
{
    public class HomeController : ASBicycleControllerBase
    {
        //[AdminLayout]
        public ActionResult Index()
        {
            var xxx = (Math.Floor((decimal.Parse("5.01") - decimal.Parse("5")) * 100) < 0
                                ? 0
                                : Math.Floor((decimal.Parse("5.01") - decimal.Parse("5")) * 100)  / 100).ToString("F");


            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}
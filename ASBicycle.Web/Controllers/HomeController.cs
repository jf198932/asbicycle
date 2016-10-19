using System.Web.Mvc;

namespace ASBicycle.Web.Controllers
{
    public class HomeController : ASBicycleControllerBase
    {
        //[AdminLayout]
        public ActionResult Index()
        {
            //return RedirectToAction("List");
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}
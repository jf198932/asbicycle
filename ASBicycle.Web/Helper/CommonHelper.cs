using System.Web;
using ASBicycle.Web.Models.Common;

namespace ASBicycle.Web.Helper
{
    public class CommonHelper
    {
        public static int? GetSchoolId()
        {
            var session = HttpContext.Current.Session["currentUser"] as BackLoginModel;
            return session?.School_id;
        }
    }
}
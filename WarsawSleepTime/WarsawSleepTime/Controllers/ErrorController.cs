using System.Web.Mvc;

namespace WarsawSleepTime.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult FourOhFour()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}

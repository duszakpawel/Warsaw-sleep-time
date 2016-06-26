using System.Linq;
using System.Web.Mvc;
using WarsawSleepTime.Entities.Context;

namespace WarsawSleepTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly WarsawSleepTimeContext context;
        public HomeController()
        {
            context = new WarsawSleepTimeContext();
        }

        public ActionResult Index()
        {
            PutNameIntoViewBag();
            return View();
        }

        public ActionResult About()
        {
            PutNameIntoViewBag();
            return View();
        }

        public ActionResult Contact()
        {
            PutNameIntoViewBag();
            return View();
        }

        public ActionResult Services()
        {
            PutNameIntoViewBag();
            return View();
        }

        #region Helpers
        private void PutNameIntoViewBag()
        {
            var firstOrDefault = context.Customers.FirstOrDefault(x => x.User.UserName == User.Identity.Name);
            if (firstOrDefault != null)
                ViewBag.LoggedUserName = firstOrDefault.FirstName;
        }
        #endregion
    }
}
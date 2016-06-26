using System.Web.Mvc;
using System.Web.Routing;

namespace WarsawSleepTime.Helpers
{
    public class AuthorizeWithAjaxAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.Result = new RedirectToRouteResult(
                               new RouteValueDictionary
                               {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                               });
            }
        }
    }
}
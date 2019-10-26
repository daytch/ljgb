using ljgb.Infrastructure.Session;
using System.Web.Mvc;

namespace ljgb.Infrastructure.WebApplication
{
    public class BaseController : Controller, IActionFilter
    {
        //public Logger AppLogger = LogManager.GetCurrentClassLogger();
        public UserSession userSession { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            userSession = SessionManager.UserSession;
            filterContext.HttpContext.Response.Headers.Remove("X-Frame-Options");
            filterContext.HttpContext.Response.AddHeader("X-Frame-Options", "AllowAll");
            base.OnActionExecuting(filterContext);
            if (userSession == null)
            {
                var apiuri = Options.LJGBSettings.WebServerUrl;
                Response.Redirect(string.Format("{0}/{1}", apiuri, "Identity/Account/Login"));
            }
            ViewBag.AppUserId = userSession.AppUserId;
            ViewBag.AppUsername = userSession.AppUsername;
            ViewBag.AppFullname = userSession.Fullname;
            ViewBag.AppEmail = userSession.Email;
            ViewBag.Gender = userSession.Gender;

        }
    }
}


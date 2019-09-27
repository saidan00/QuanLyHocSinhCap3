using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QuanLyHocSinhCap3.Helpers;

namespace QuanLyHocSinhCap3.Controllers
{
    public class BaseController : Controller
    {
        /*  override OnActionExecuting method
         *  before calling a Action in Controller
         *  check if user is logged in or not
         *  if not then redirect to login page
         */
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //SessionHelper sessionHelper = new SessionHelper();
            //if (!sessionHelper.IsLoggedIn())
            //{
            //    // if user is not logged in
            //    filterContext.Result = new RedirectResult(Url.Action("Login", "Account"));
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}
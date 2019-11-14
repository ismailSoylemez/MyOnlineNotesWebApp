using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Filters
{
    public class Exc : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["LastError"] = filterContext.Exception;

            filterContext.ExceptionHandled = true;//hatayı ben yöneteceğim demek.
            filterContext.Result = new RedirectResult("/Home/HasError");

        }
    }
}
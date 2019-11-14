using MyOnlineNotesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Filters
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //kullanıcı login olmamışsa
            if (CurrentSession.User == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
            

        }
    }
}
using MyOnlineNotesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Filters
{

    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        //Kullanıcı admin değilse
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User != null && CurrentSession.User.IsAdmin == false)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}
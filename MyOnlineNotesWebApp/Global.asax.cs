using MyOnlineNotes.Common;
using MyOnlineNotesWebApp.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyOnlineNotesWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //web uygulamsý ayaða kalktýðýnda Ýlk olarak Global asax çalýþýr
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            App.Common = new WebCommon();
        }
    }
}

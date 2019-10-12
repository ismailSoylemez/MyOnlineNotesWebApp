using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            

            MyOnlineNotes.BusinessLayer.Test test = new MyOnlineNotes.BusinessLayer.Test();

            return View();
        }
    }
}
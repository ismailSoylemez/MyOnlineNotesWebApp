using MyOnlineNotes.BusinessLayer;
using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MyOnlineNotesWebApp.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            //MyOnlineNotes.BusinessLayer.Test test = new MyOnlineNotes.BusinessLayer.Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.CommentTest();

            //if (TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();
            return View(nm.GetAllNote());
        }


        public ActionResult ByCategory(int? id)
        {
            //id si yoksa
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);//

            //Category bulunamadıysa..
            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }

           //bu modeli index view ına gönderdi
            return View("Index",cat.Notes);
        }







    }
}
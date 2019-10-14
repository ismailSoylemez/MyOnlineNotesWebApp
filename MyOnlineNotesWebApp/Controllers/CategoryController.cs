using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using MyOnlineNotes.BusinessLayer;
using MyOnlineNotesEntities;

namespace MyOnlineNotesWebApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Categoryt
        //public ActionResult Select(int? id)
        //{
        //    //id si yoksa
        //    if (id==null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    CategoryManager cm = new CategoryManager();
        //    Category cat = cm.GetCategoryById(id.Value);//

        //    //Category bulunamadıysa..
        //    if (cat ==null)
        //    {
        //        return HttpNotFound();
        //        //return RedirectToAction("Index", "Home");
        //    }

        //    TempData["mm"] = cat.Notes;
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
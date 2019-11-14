using MyOnlineNotes.BusinessLayer;
using MyOnlineNotes.BusinessLayer.Results;
using MyOnlineNotesEntities;
using MyOnlineNotesWebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    public class UserController : Controller
    {
        private MyOnlineNotesUserManager myOnlineNotesUserManager = new MyOnlineNotesUserManager();
        private OnlineNoteUser onlineNoteUser = new OnlineNoteUser();


        // GET: User
        public ActionResult Index()
        {
            return View(myOnlineNotesUserManager.List());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            onlineNoteUser = myOnlineNotesUserManager.Find(x => x.Id == id.Value);

            if (onlineNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(onlineNoteUser);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(OnlineNoteUser onlineNoteUser)
        {
            //bu bilgieri kontrol etme
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<OnlineNoteUser> res = myOnlineNotesUserManager.Insert(onlineNoteUser);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(onlineNoteUser);
                                    }

                //insert yapmadan önce kontrol gerekli
                return RedirectToAction("Index");

            }

            return View(onlineNoteUser);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            onlineNoteUser = myOnlineNotesUserManager.Find(x => x.Id == id.Value);

            if (onlineNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(onlineNoteUser);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(OnlineNoteUser onlineNoteUser)
        {
            //bu bilgieri kontrol etme
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<OnlineNoteUser> res = myOnlineNotesUserManager.Update(onlineNoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(onlineNoteUser);
                }
                return RedirectToAction("Index");

            }


            return View(onlineNoteUser);


        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            onlineNoteUser = myOnlineNotesUserManager.Find(x => x.Id == id.Value);
            if (onlineNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(onlineNoteUser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            onlineNoteUser = myOnlineNotesUserManager.Find(x => x.Id == id);
            myOnlineNotesUserManager.Delete(onlineNoteUser);
            return RedirectToAction("Index");
        }
    }
}

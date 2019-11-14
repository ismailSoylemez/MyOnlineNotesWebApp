using MyOnlineNotes.BusinessLayer;
using MyOnlineNotesEntities;
using MyOnlineNotesWebApp.Filters;
using MyOnlineNotesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Controllers
{
    public class NoteController : Controller
    {

        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikedManager likedManager = new LikedManager();

        [Auth]
        public ActionResult Index()
        {
            //buraya giren kişi kendi notlarını görecek
            //sessiondaki kişinin id sine göre notları getireceğiz
            //devamlı session kontrolü yapmamak için web tarafında class oluşturacağız(Model içinde)

            //kategori bilgisiyle getirsin(join atıyor)
            var notes = noteManager.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(x=>x.ModifiedOn);
            

            return View(notes.ToList());
        }

        [Auth]
        public ActionResult MyLikedNotes()
        {
            //Linq sorgusu
            var notes = likedManager.ListQueryable().Include("LikedUser")
                .Include("Note")
                .Where(x => x.LikedUser.Id == CurrentSession.User.Id)
                .Select(x=>x.Note)
                .Include("Category")
                .Include("Owner")
                .OrderByDescending(x=>x.ModifiedOn);

            return View("Index", notes.ToList());
        }

        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = noteManager.Find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                noteManager.Insert(note);
                return RedirectToAction("Index");

            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View(note);


        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note  = noteManager.Find(x => x.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View(note);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Note db_note = noteManager.Find(x => x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;
                db_note.CategoryId = note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;

                noteManager.Update(db_note);

                return RedirectToAction("Index");

            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View(note);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        [Auth]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Note note = noteManager.Find(x => x.Id == id);
            noteManager.Delete(note);

            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {

            //sadece kullanıcının likeladığı notların idlerini döndüreceğim. bunun için sorgu yazmalıyım

            List<int> likedNoteIds = likedManager.List(
                x => x.LikedUser.Id == CurrentSession.User.Id  && ids.Contains(x.Note.Id)).Select(
                x =>x.Note.Id).ToList();



            return Json(new { result = likedNoteIds});



        }


        [HttpPost]
        public ActionResult SetLikeState(int noteid , bool liked )
        {

            int res = 0;

            //kullanıcının likelamaya çalıştığı not mevcut mu ?
            Liked like = likedManager.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.User.Id);


            Note note = noteManager.Find(x => x.Id == noteid); // notu aldık

            if (like != null && liked == false) // kayıtı silmem gerekir
            {
               res = likedManager.Delete(like);
            }
            else if (like == null && liked == true) //böyle bir kayıt yok ise kullanıcı ilk defa like atıyordur
            {
                res = likedManager.Insert(new Liked() { // insert yaptım
                    LikedUser= CurrentSession.User,
                    Note=note
                });
            }


            if (res > 0) // bir işlem yaptıysam
            {
                if (liked) // like attıysam
                {
                    note.LikeCount++;
                }
                else // like kaldırdıysam
                {
                    note.LikeCount--;
                }

                res = noteManager.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount }); // işlem başarılı olduysa
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount }); //işlem başarısız olduğunda sayafaya dönecek değer

        }


        public ActionResult GetNoteText(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = noteManager.Find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }


            return PartialView("_PartialNoteText", note);
        }


    }
}

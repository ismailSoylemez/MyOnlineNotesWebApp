using MyOnlineNotes.BusinessLayer;
using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Controllers
{
    public class CommentController : Controller
    {

        private NoteManager noteManager = new NoteManager(); 

        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Note note = noteManager.Find(x => x.Id == id);
            Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);





            if (note == null)
            {
                return HttpNotFound();
            }


            return PartialView("_PartialComments",note.Comments);
        } 
    }
}
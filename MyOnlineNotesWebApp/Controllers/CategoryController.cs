using MyOnlineNotes.BusinessLayer;
using MyOnlineNotesEntities;
using MyOnlineNotesWebApp.Filters;
using MyOnlineNotesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineNotesWebApp.Controllers
{
    [Exc]
    [Auth]//Filter işlemi: kullanıcı login değilse logine yönlendir
    [AuthAdmin]
    public class CategoryController : Controller
    {

        private CategoryManager categoryManager = new CategoryManager();


        // GET: Category
        public ActionResult Index()
        {
            return View(CacheHelper.GetCategoriesFromCache());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x=>x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");



            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);


                //insert update delete işlemlerinden sonra cache güncellemesi yapmamız gerekir çünkü güncellenmezse eski dataları çekecektir
                CacheHelper.RemoveCategoiesFromCache();


                return RedirectToAction("Index");

            }

            return View(category);

            
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                Category cat = categoryManager.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;


                // TODO : İNCELE
                categoryManager.Update(cat);
                CacheHelper.RemoveCategoiesFromCache();


                return RedirectToAction("Index");

            }

            return View(category);

        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {  
            Category category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(category);
            CacheHelper.RemoveCategoiesFromCache();

            return RedirectToAction("Index");

        }
    }
}

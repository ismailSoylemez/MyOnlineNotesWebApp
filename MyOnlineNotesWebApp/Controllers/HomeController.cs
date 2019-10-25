using MyOnlineNotes.BusinessLayer;
using MyOnlineNotes.DataAccessLayer.EntityFramework;
using MyOnlineNotesEntities;
using MyOnlineNotesEntities.Messages;
using MyOnlineNotesEntities.ValueObject;
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
            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());//veriyi sqlden çektik tersten sıraladık
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//veriyi sqlden çektik sıraladık
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
            return View("Index",cat.Notes.OrderByDescending(x=>x.ModifiedOn).ToList());
        }


        public ActionResult MostLiked()
        {

            NoteManager nm = new NoteManager();
            return View("Index", nm.GetAllNote().OrderByDescending(x=>x.LikeCount).ToList());



        }

        public ActionResult About()
        {
            return View();
        }





        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //giriş kontrolü ve yönlendirme
            // sessionda kullanıcı bilgisi saklama     

            if (ModelState.IsValid) //gelen model uygunsa
            {
                MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
                BusinessLayerResult<OnlineNoteUser> res = eum.LoginUser(model);

                if (res.Errors.Count>0) //hata vardır
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    //eğer kullanıcı aktif değil kodunu içeriyorsa dönen hata mesajı değiştirilebilir
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-7890";
                    }



                    return View(model);
                }

                Session["login"] = res.Result;
               
                //hata yoksa
                return RedirectToAction("Index");




            }





            return View();
        }



        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //bu kontrolleri burada yapamazsın BusinessLayerda yapman gerekiyor
            //kullanıcı username kontrolü 
            //kullanıcı eposta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi

            //gelen model geçerli mi ?
            if (ModelState.IsValid)
            {

                MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
                BusinessLayerResult<OnlineNoteUser> res = eum.RegisterUser(model);


                //hata varsa
                if (res.Errors.Count>0) 
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View(model);
                }

                //hata yoksa
                return RedirectToAction("RegisterOk");

            }




            return View(model);
        }



        public ActionResult UserActivate(Guid activate )
        {
            //kullanıcı aktivasyonu sağlanacak
            return View();
        }


        
        public ActionResult RegisterOk()
        {
            return View();
        }







        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }






    }
}
using MyOnlineNotes.BusinessLayer;
using MyOnlineNotes.BusinessLayer.Results;
using MyOnlineNotes.DataAccessLayer.EntityFramework;
using MyOnlineNotesEntities;
using MyOnlineNotesEntities.Messages;
using MyOnlineNotesEntities.ValueObject;
using MyOnlineNotesWebApp.Filters;
using MyOnlineNotesWebApp.Models;
using MyOnlineNotesWebApp.ViewModels;
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
        private NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        MyOnlineNotesUserManager OnlineNoteUserManager = new MyOnlineNotesUserManager();





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
            
            //NoteManager nm = new NoteManager();
            
            return View(noteManager.ListQueryable().Where(x=>x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());//veriyi sqlden çektik tersten sıraladık
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());//veriyi sqlden çektik sıraladık
        }


        public ActionResult ByCategory(int? id)
        {
            //id si yoksa
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Category cat = categoryManager.Find(x=>x.Id==id.Value);//

            //Category bulunamadıysa..
            /*if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }*/

            //List<Note> notes = cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList();

            List<Note> notes = noteManager.ListQueryable().Where(x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(x => x.ModifiedOn).ToList();

           //bu modeli index view ına gönderdi
            return View("Index",notes);
        }


        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x=>x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }


        [Auth]
        public ActionResult ShowProfile()
        {
            //artık buna gerek yok çünkü CurrentUser classında tutuyoruz bu bilgiyi
            //OnlineNoteUser currentUser = Session["login"] as OnlineNoteUser;

            BusinessLayerResult<OnlineNoteUser> res = OnlineNoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count>0)
            {

                ErrorViewModel ErrornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                };
                return RedirectToAction("Error", ErrornotifyObj);

            }

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            //OnlineNoteUser currentUser = Session["login"] as OnlineNoteUser;

            //MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
            BusinessLayerResult<OnlineNoteUser> res = OnlineNoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {

                ErrorViewModel ErrornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                };
                return RedirectToAction("Error", ErrornotifyObj);

            }

            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(OnlineNoteUser model, HttpPostedFileBase ProfileImage)
        {

            //eğer böyle bir item varsa siler sonra kontrol yapar
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {

                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    //varsayılan kullanıcı foto yolu
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";



                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
                BusinessLayerResult<OnlineNoteUser> res = eum.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncelleme Başarısız",
                        RedirectUrl = "/Home/EditProfile"
                    };

                    return View("Error", errNotifyObj);

                }

                CurrentSession.Set<OnlineNoteUser>("login", res.Result); //session güncellendi

                return RedirectToAction("ShowProfile");
            }


            return View(model);






            //Repository<OnlineNoteUser> repo_user = new Repository<OnlineNoteUser>();

            //OnlineNoteUser res = repo_user.Find(x => x.Id == user.Id);

            //if (res != null)
            //{
            //    res.Name = user.Name;
            //    res.Surname = user.Surname;
            //    res.Email = user.Email;
            //    res.Username = user.Username;
            //    res.Password = user.Password;

            //}

            //repo_user.Update(res);


        }

        [Auth] 
        public ActionResult DeleteProfile()
        {
            //OnlineNoteUser currentUser = Session["login"] as OnlineNoteUser;

            //MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
            BusinessLayerResult<OnlineNoteUser> res = OnlineNoteUserManager.RemoveUserById(CurrentSession.User.Id );

            if (res.Errors.Count>0)
            {
                ErrorViewModel errNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectUrl = "/Home/ShowProfile"
                };
                return View("Error", errNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");

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
                //MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
                BusinessLayerResult<OnlineNoteUser> res = OnlineNoteUserManager.LoginUser(model);

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

                CurrentSession.Set<OnlineNoteUser>("login", res.Result);
               
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

                //MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
                BusinessLayerResult<OnlineNoteUser> res = OnlineNoteUserManager.RegisterUser(model);


                //hata varsa
                if (res.Errors.Count>0) 
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View(model);
                }


                //bu noktaya kadar geldiyse modeli oluştur ve ilgili view e gönder
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Lütfen Eposta adresinize giderek hesabınızı aktive ediniz.");

                //hata yoksa
                return View("Ok", notifyObj);

            }




            return View(model);
        }




        public ActionResult UserActivate(Guid id )
        {
            //kullanıcı aktivasyonu sağlanacak

            //MyOnlineNotesUserManager eum = new MyOnlineNotesUserManager();
            BusinessLayerResult<OnlineNoteUser> res = OnlineNoteUserManager.ActivatedUser(id);

            if (res.Errors.Count>0)
            {
                // ?
                TempData["errors"] = res.Errors;

                ErrorViewModel ErrornotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors,
                };
                return RedirectToAction("Error", ErrornotifyObj);
            }


            //eğer işlem başarılıysa
            OkViewModel OknotifyObj = new OkViewModel()
            {
                Title = "Aktivasyon Başarılı",
                RedirectUrl="/Home/Login",
            };
            OknotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşımı yapabilirsiniz");

            return View("Ok", OknotifyObj);
        }




        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }



        public ActionResult AccessDenied()
        {
            return View();
        }




    }
}
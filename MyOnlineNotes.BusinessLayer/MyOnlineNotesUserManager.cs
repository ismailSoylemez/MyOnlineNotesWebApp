﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOnlineNotes.BusinessLayer.Abstract;
using MyOnlineNotes.BusinessLayer.Results;
using MyOnlineNotes.Common.Helpers;
using MyOnlineNotes.DataAccessLayer.EntityFramework;
using MyOnlineNotesEntities;
using MyOnlineNotesEntities.Messages;
using MyOnlineNotesEntities.ValueObject;

namespace MyOnlineNotes.BusinessLayer
{
    public class MyOnlineNotesUserManager : ManagerBase<OnlineNoteUser>
    {


        public  BusinessLayerResult<OnlineNoteUser> RegisterUser (RegisterViewModel data)
        {
            //kullanıcı username kontrolü 
            //kullanıcı eposta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi
            OnlineNoteUser user =Find(x => x.Username == data.Username || x.Email == data.EMail);
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();


            if (user!=null)
            {
                //birden fazla hata mesajı ekleyebileceğim bir yapı oluşturdum
                if(user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Email adresi kayıtlı");
                }
            }
            else//eşleşen kullanıcı yoksa kullanıcıyı database ye ekleyecek
            {
                int dbResult = base.Insert(new OnlineNoteUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ActivatedGuid = Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false,
                    ProfileImageFileName="DefaultProfileImage.png"
                });
                if (dbResult > 0) //başarılıysa
                {
                    //kullanıcı insert olmuş demektir
                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    //TODO : aktivasyon maili atılacak
                    //layerResult.Result.ActivatedGuid();

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivatedGuid}";//linkimiz
                    string body = $"Merhaba {res.Result.Username};<br/><br/>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank' >tıklayınız</a>.";


                    MailHelper.SendMail(body, res.Result.Email,"Hesap Aktifleştirme");

                    


                }

            }

            //hatalar var layerresult geri döndürülür.
            return res;


        }



        public  BusinessLayerResult<OnlineNoteUser> GetUserById(int id)
        {
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();

            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }

            return res;
        }

        public  BusinessLayerResult<OnlineNoteUser> LoginUser (LoginViewModel data)
        {
            //giriş kontrolü 
            //hesap aktif mi ?
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);



            if (res.Result != null)//database de eşleşme sağlanmışsa
            {
                if (!res.Result.IsActive)//kullanıcı aktif değilse
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktif değil.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Email adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor");
            }

            return res;



        }

        public  BusinessLayerResult<OnlineNoteUser> ActivatedUser(Guid activateId)
        {
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();
            res.Result = Find(x => x.ActivatedGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir!");
                    return res;
                }

                //eğer aktif değilse
                res.Result.IsActive = true;
                Update(res.Result);

            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirmek için kullanıcı bulunamadı ");

            }

            return res;


        }

        public  BusinessLayerResult<OnlineNoteUser> UpdateProfile(OnlineNoteUser data)
        {

            OnlineNoteUser db_user = Find(x => x.Username == data.Username && x.Email == data.Email);
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();

            if (db_user!=null &&  db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı !");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E posta adresi kayıtlı !");

                }
                return res;
            }


            //hata yoksa
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Password = data.Password;
            res.Result.Surname = data.Surname;
            

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (base.Update(res.Result) == 0 )
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi !");
            }

            return res;
        }

        public  BusinessLayerResult<OnlineNoteUser> RemoveUserById(int id)
        {
            OnlineNoteUser user = Find(x => x.Id == id);
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();


            if (user != null)
            {

                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                    return res;
                }
            }

            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı");
            }

            return res;


        }




        // user insert update için abstract sınıf olan managerbaseden miras alıp değiştireceğiz çünkü şuanki insert metodu kullanıcıyı direkt olarak insert ediyor içinde kontrol yapamıyorum.
        // benim insert metodum geriye int dönüyor bunu istemiyorum.


        //istediğim tipi dönen insert yazıyorum
        //new verdiğimiz zaman bu metodu gizleriz 
        public  new BusinessLayerResult<OnlineNoteUser> Insert(OnlineNoteUser data)
        {
            //method gizleme,


            OnlineNoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();

            res.Result = data;

            if (user != null)
            {
                //birden fazla hata mesajı ekleyebileceğim bir yapı oluşturdum
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Email adresi kayıtlı");
                }
            }
            else//eşleşen kullanıcı yoksa kullanıcıyı database ye ekleyecek
            {
                res.Result.ProfileImageFileName = "";
                res.Result.ActivatedGuid = Guid.NewGuid();

                if (base.Insert(res.Result)==0) // diğerleri varsayılan olarak zaten gelecek)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi");
                }
            }

            //hatalar var layerresult geri döndürülür.
            return res;





        }


        public new BusinessLayerResult<OnlineNoteUser> Update(OnlineNoteUser data)
        {

            OnlineNoteUser db_user = Find(x => x.Username == data.Username && x.Email == data.Email);
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();

            res.Result = data;


            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı !");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E posta adresi kayıtlı !");

                }
                return res;
            }


            //hata yoksa
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Password = data.Password;
            res.Result.Surname = data.Surname;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;


    

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi !");
            }

            return res;

        }





    }
}

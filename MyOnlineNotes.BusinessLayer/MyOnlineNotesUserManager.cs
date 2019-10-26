using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOnlineNotes.Common.Helpers;
using MyOnlineNotes.DataAccessLayer.EntityFramework;
using MyOnlineNotesEntities;
using MyOnlineNotesEntities.Messages;
using MyOnlineNotesEntities.ValueObject;

namespace MyOnlineNotes.BusinessLayer
{
    public class MyOnlineNotesUserManager
    {
        private Repository<OnlineNoteUser> repo_user = new Repository<OnlineNoteUser>();


        public BusinessLayerResult<OnlineNoteUser> RegisterUser (RegisterViewModel data)
        {
            //kullanıcı username kontrolü 
            //kullanıcı eposta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi
            OnlineNoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.EMail);
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
                int dbResult = repo_user.Insert(new OnlineNoteUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ActivatedGuid = Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false,

                });
                if (dbResult > 0) //başarılıysa
                {
                    //kullanıcı insert olmuş demektir
                    res.Result = repo_user.Find(x => x.Email == data.EMail && x.Username == data.Username);

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



        public BusinessLayerResult<OnlineNoteUser> GetUserById(int id)
        {
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();

            res.Result = repo_user.Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }

            return res;
        }

        public BusinessLayerResult<OnlineNoteUser> LoginUser (LoginViewModel data)
        {
            //giriş kontrolü 
            //hesap aktif mi ?
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();
            res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password);



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



        public BusinessLayerResult<OnlineNoteUser> ActivatedUser(Guid activateId)
        {
            BusinessLayerResult<OnlineNoteUser> res = new BusinessLayerResult<OnlineNoteUser>();
            res.Result = repo_user.Find(x => x.ActivatedGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir!");
                    return res;
                }

                //eğer aktif değilse
                res.Result.IsActive = true;
                repo_user.Update(res.Result);

            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirmek için kullanıcı bulunamadı ");

            }

            return res;


        }


    }
}

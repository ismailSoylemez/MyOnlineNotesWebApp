using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOnlineNotes.DataAccessLayer.EntityFramework;
using MyOnlineNotesEntities;
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
            BusinessLayerResult<OnlineNoteUser> layerResult = new BusinessLayerResult<OnlineNoteUser>();


            if (user!=null)
            {
                //birden fazla hata mesajı ekleyebileceğim bir yapı oluşturdum
                if(user.Username == data.Username)
                {
                    layerResult.Errors.Add("Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.EMail)
                {
                    layerResult.Errors.Add("Eposta adresi kayıtlı");
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
                    repo_user.Find(x => x.Email == data.EMail && x.Username == data.Username);

                    //TODO : aktivasyon maili atılacak
                    //layerResult.Result.ActivatedGuid();
                }

            }

            //hatalar var layerresult geri döndürülür.
            return layerResult;


        }


    }
}

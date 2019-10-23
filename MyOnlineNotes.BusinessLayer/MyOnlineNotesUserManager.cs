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


        public OnlineNoteUser RegisterUser(RegisterViewModel data)
        {
            //kullanıcı username kontrolü 
            //kullanıcı eposta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi
            OnlineNoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.EMail);

            if (user!=null)
            {
                throw new Exception("Kayıtlı kullanıcı adı ya da e-posta") //bu hatayı homecontrollerda yakalamam gerekir...
            }


        }


    }
}

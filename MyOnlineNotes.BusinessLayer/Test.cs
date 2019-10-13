using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOnlineNotes.BusinessLayer
{
    public class Test
    {
        private Repository<OnlineNoteUser> repo_user = new Repository<OnlineNoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        

        public Test()
        {
            //DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            //db.Categories.ToList();//örnek dataların oluşması için tabloya seed atmamız gerekiyor..
            List<Category> categories = repo_category.List();
        }

        //yeni kullanıcı oluşturuyorum
        public void InsertTest()
        {
            int result = repo_user.Insert(new OnlineNoteUser()
            {
                Name = "aaa",
                Surname = "bbb",
                Email = "aaa@gmail.com",
                ActivatedGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "aabb",
                Password = "111",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "aabb"
            });
        }


        public void UpdateTest()
        {
            OnlineNoteUser user = repo_user.Find(x => x.Username == "aabb");
            if(user != null)
            {
                user.Username = "xxx";
               int result = repo_user.Save();
            }


        }

        public void DeleteTest()
        {
            OnlineNoteUser user = repo_user.Find(x => x.Username == "xxx");
            if (user != null)
            {
                repo_user.Delete(user);
                int result = repo_user.Save();
            }


        }

    }
}

using MyOnlineNotes.DataAccessLayer;
using MyOnlineNotes.DataAccessLayer.EntityFramework;
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
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();


        public Test()
        {
            //DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            //db.Categories.ToList();//örnek dataların oluşması için tabloya seed atmamız gerekiyor..
            List<Category> categories = repo_category.List();
            List<Category> categories_filtered = repo_category.List(x => x.Id > 5);

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
               int result = repo_user.Update(user);
            }


        }

        public void DeleteTest()
        {
            OnlineNoteUser user = repo_user.Find(x => x.Username == "xxx");
            if (user != null)
            {
                repo_user.Delete(user);
                int result = repo_user.Update(user);
            }


        }




        //Bağlantılı tablolarda yorum ekleme senaryosu
        public void CommentTest()
        {
            //id si 1 olan kullanıcıyı buluyor..
            OnlineNoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);


            Comment comment = new Comment()
            {
                Text = "bu bir testtir",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "ismailsoylemez",
                Note = note,
                Owner = user
            };
            repo_comment.Insert(comment);

        }






    }
}

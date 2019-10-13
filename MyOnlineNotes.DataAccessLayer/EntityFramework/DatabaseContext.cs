using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.Entity;
using MyOnlineNotesEntities;

namespace MyOnlineNotes.DataAccessLayer.EntityFramework
{
    public class DatabaseContext :DbContext
    {

        //örnek dataları basıyoruz
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //diğer projelerdeki classlara erişmek için References ' a entities dll eklemek gerekiyor
        public DbSet<OnlineNoteUser> OnlineNoteUser { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        //connectionstringi el ile web configde oluştur

    }
}

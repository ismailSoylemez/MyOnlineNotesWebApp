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



        //tablolar oluşmadan önce araya girer
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Fluent API yöntemi

            //note tablosu comment ile 1 e çok
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Comments)
                .WithRequired(c => c.Note) //bir commentin notu olmak zorundadır
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes)
                .WithRequired(c => c.Note) //bir like ın notu olmak zorundadır
                .WillCascadeOnDelete(true);


                


            //user delete
            //modelBuilder.Entity<OnlineNoteUser>()
            //    .HasMany(n => n.Comments)
            //    .WithRequired(c => c.Owner)
            //    .WillCascadeOnDelete(true);
            //modelBuilder.Entity<OnlineNoteUser>()
            //    .HasMany(n => n.Likes)
            //    .WithRequired(c => c.LikedUser)
            //    .WillCascadeOnDelete(true);
            //modelBuilder.Entity<OnlineNoteUser>()
            //   .HasMany(n => n.Notes)
            //   .WithRequired(c => c.Owner)
            //   .WillCascadeOnDelete(true);





            //modelBuilder.Entity<OnlineNoteUser>()
            //    .HasMany(n => n.Notes)
            //    .WithRequired(c => c.Owner)
            //    .WillCascadeOnDelete(true);





        }

    }
}

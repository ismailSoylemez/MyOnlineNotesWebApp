using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyOnlineNotesEntities;

namespace MyOnlineNotes.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {

        //örnek data basacağız
        protected override void Seed(DatabaseContext context)
        {
            // admin ekleme
            OnlineNoteUser admin = new OnlineNoteUser()
            {
                Name = "İsmail",
                Surname= "Söylemez",
                Email="ismailsoylemez272@gmail.com",
                ActivatedGuid=Guid.NewGuid(),
                IsActive=true,
                IsAdmin=true,
                Username="ismailsoylemez",
                Password="123456",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now.AddMinutes(5),
                ModifiedUsername="ismailsoylemez",
                ProfileImageFileName="DefaultProfileImage.png"
            };

            //standart user ekleme
            OnlineNoteUser standartuser = new OnlineNoteUser()
            {
                Name = "Merve",
                Surname = "Er",
                Email = "merve@gmail.com",
                ActivatedGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "merve",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "ismailsoylemez",
                ProfileImageFileName = "DefaultProfileImage.png"

            };
            context.OnlineNoteUser.Add(admin);
            context.OnlineNoteUser.Add(standartuser);

            //kullanıcı ekleniyor
            for (int i = 0; i < 8; i++)
            {
                OnlineNoteUser user = new OnlineNoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivatedGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}",
                    ProfileImageFileName = "DefaultProfileImage.png"

                };
                context.OnlineNoteUser.Add(user);
            }
            context.SaveChanges();



            List<OnlineNoteUser> userlist = context.OnlineNoteUser.ToList();


            //örnek kategori ekleniyor
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description=FakeData.PlaceData.GetAddress(),
                    CreatedOn=DateTime.Now,
                    ModifiedOn=DateTime.Now,
                    ModifiedUsername= "ismailsoylemez"
                };
                context.Categories.Add(cat);

                //kategorilerin notları ekleniyor
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,10); k++)
                {
                    OnlineNoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner= owner,
                        CreatedOn =FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    cat.Notes.Add(note);

                    //notların commentleri ekleniyor
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        OnlineNoteUser commentOwner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = commentOwner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = commentOwner.Username
                        };
                        note.Comments.Add(comment);
                    }


                    //Likelar ekleniyor
                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };
                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();

        }

    }
}

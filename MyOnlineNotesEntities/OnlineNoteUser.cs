using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyOnlineNotesEntities
{
    [Table("OnlineNoteUsers")]
    public class OnlineNoteUser : MyEntityBase
    {
        [StringLength(25),DisplayName("İsim")]
        public string Name { get; set; }

        [StringLength(25), DisplayName("Soyad")]
        public string Surname { get; set; }

        [Required,StringLength(25), DisplayName("Kullanıcı Adı")]
        public string Username { get; set; }

        [Required, StringLength(70), DisplayName("E Posta")]
        public string Email { get; set; }

        [Required, StringLength(25), DisplayName("Şifre")]
        public string Password { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        [Required,ScaffoldColumn(false)]
        public Guid ActivatedGuid { get; set; }

        [StringLength(30), ScaffoldColumn(false)]   //images/user_12.jpg
        public string ProfileImageFileName { get; set; }


        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}

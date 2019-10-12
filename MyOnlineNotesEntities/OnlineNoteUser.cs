using System;
using System.Collections.Generic;
using System.Text;

namespace MyOnlineNotesEntities
{
    public class OnlineNoteUser : MyEntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid ActivatedGuid { get; set; }
        public bool IsAdmin { get; set; }



        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}

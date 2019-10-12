using System;
using System.Collections.Generic;
using System.Text;

namespace MyOnlineNotesEntities
{
   public class Liked
    {
        public int Id { get; set; }
        public virtual Note Note { get; set; }
        public virtual OnlineNoteUser LikedUser { get; set; }
    }
}

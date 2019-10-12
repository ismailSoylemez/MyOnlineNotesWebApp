using System;
using System.Collections.Generic;
using System.Text;

namespace MyOnlineNotesEntities
{
    public class Note : MyEntityBase
    {

        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public int CategoryId { get; set; }

        public virtual OnlineNoteUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MyOnlineNotesEntities
{
    public class Comment : MyEntityBase
    {
        public string String { get; set; }




        public virtual Note Note  { get; set; }
        public virtual OnlineNoteUser  Owner { get; set; }
    }
}

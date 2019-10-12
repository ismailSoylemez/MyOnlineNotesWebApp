using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyOnlineNotesEntities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {

        [Required, StringLength(300)]
        public string Text { get; set; }




        public virtual Note Note  { get; set; }
        public virtual OnlineNoteUser  Owner { get; set; }
    }
}

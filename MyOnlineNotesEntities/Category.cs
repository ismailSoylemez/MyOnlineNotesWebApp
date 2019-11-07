using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyOnlineNotesEntities
{
    [Table("Categories")]
   public  class Category : MyEntityBase
    {
        [DisplayName("Kategori"),Required,StringLength(50)]
        public string Title { get; set; }

        [DisplayName("Açıklama"),StringLength(150)]
        public string Description { get; set; }


        public virtual List<Note> Notes { get; set; }



        public Category()
        {
            Notes = new List<Note>();
        }




    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MyOnlineNotesEntities
{
    public class MyEntityBase
    {

        public int Id { get; set; }



        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedUsername { get; set; }

    }
}

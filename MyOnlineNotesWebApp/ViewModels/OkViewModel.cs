using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.ViewModels
{
    public class OkViewModel:NotifyViewModalBase<string>
    {

        public OkViewModel()
        {
            Title = "İşlem Başarılı";

        }


    }
}
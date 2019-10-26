using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.ViewModels
{
    public class WarningViewModel:NotifyViewModalBase<string>
    {

        public WarningViewModel()
        {
            Title = "Uyarı!";
        }


    }
}
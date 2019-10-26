using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.ViewModels
{
    public class NotifyViewModalBase<T>
    {
        public List<T> Items { get; set; }

        public string Header { get; set; }

        public string Title { get; set; }

        public bool IsRedirecting { get; set; }

        public string RedirectUrl { get; set; }

        public int RedirectingTimeout { get; set; }


        // Default değerler
        public NotifyViewModalBase()
        {
            Header = "Yönlendiriliyorsunuz";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectUrl = "/Home/Index";
            RedirectingTimeout = 10000;
            Items = new List<T>();
        }


    }
}
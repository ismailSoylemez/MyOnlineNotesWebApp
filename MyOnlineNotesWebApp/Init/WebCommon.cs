using MyOnlineNotes.Common;
using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {

            if (HttpContext.Current.Session["Login"] != null)
            {
                OnlineNoteUser user = HttpContext.Current.Session["Login"] as OnlineNoteUser;
                return user.Username;
            }

            return null;

        }
    }
}
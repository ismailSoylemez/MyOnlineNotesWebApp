using MyOnlineNotes.Common;
using MyOnlineNotesEntities;
using MyOnlineNotesWebApp.Models;
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
            OnlineNoteUser user = CurrentSession.User;

            if (user != null)
            {
                return user.Username;
            }

            else 
            return "system";

        }
    }
}
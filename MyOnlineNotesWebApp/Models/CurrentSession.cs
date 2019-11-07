using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.Models
{
    public  class CurrentSession
    {

        public static  OnlineNoteUser User
        {
            get {

                return Get<OnlineNoteUser>("login");
            }
        }


        public static void Set<T> (string key , T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }


        public static void Remove(string key)
        {
            if (HttpContext.Current.Session["key"] != null)
            {
                 HttpContext.Current.Session.Remove(key);
            }
        }


        public static void  Clear()
        {
            if (HttpContext.Current.Session["key"] != null)
            {
                HttpContext.Current.Session.Clear();
            }
        }


    }
}
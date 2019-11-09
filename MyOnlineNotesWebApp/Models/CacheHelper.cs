using MyOnlineNotes.BusinessLayer;
using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyOnlineNotesWebApp.Models
{
    public class CacheHelper
    {
        //bu metot sayesinde kategorileri çekeceğim. sürekli veritabanına istek atmaktan kurtulacağım
        //WebCache den faydalanacağım
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");

            //eğer cache boşsa ilk olarak verileri çekmem lazım
            if (result==null)
            {
                CategoryManager categoryManager = new CategoryManager();

                result = categoryManager.List();

                WebCache.Set("category-cache",result,20,true);
            }

            return result;
            

        }


        public static void RemoveCategoiesFromCache()
        {
            Remove("category-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }

    }
}
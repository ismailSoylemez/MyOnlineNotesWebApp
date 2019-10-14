using MyOnlineNotes.DataAccessLayer.EntityFramework;
using MyOnlineNotesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOnlineNotes.BusinessLayer
{
    public class CategoryManager
    {
        private Repository<Category> repo_category = new Repository<Category>();

        public List<Category> GetCategories()
        {
            return repo_category.List();
        }

        //verilen id ye ait kategori var mı ?
        public Category GetCategoryById(int id)
        {
            return repo_category.Find(x => x.Id == id);

        }



    }
}

using MyOnlineNotesEntities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOnlineNotes.DataAccessLayer;
using System.Data.Entity;
using System.Linq.Expressions;

namespace MyOnlineNotes.BusinessLayer
{


    //T class olmak zorundadır..
    //tüm classlar için ayrı fonk. yerine generic tanımladık

    public class Repository<T> : RepositoryBase where T : class
    {

        //RepositoryBase sınıfından miras aldığım için o class içinde dbcontext oluşuyor.Buradaki dbler hata vermeyecek çünkü miras alınan sınıftaki db yi kullanıyorlar

        //private DatabaseContext db;
        //her fonksiyonda ayrı ayrı set etmek yerine bunu yapıyoruz
        private DbSet<T> _objectSet;


        //constructorda ilk olarak ilgili set bulunuyor. ne verirsen onu buluyor.
        //böylece her seferinde veritabanında gereksiz işlem önleniyor
        public Repository()
        {
            //db =RepositoryBase.CreateContext();//database context oluşumu
            _objectSet = db.Set<T>();
        }

        //listeleme methodu
        public List<T> List()
        {
            //ilgili dbseti elde etmeye çalşıyor
            return _objectSet.ToList();
        } 

        //kritere göre listeleme
        public List<T> List(Expression<Func<T,bool>> where )
        {
            return _objectSet.Where(where).ToList();
        }

        //insert methodu
        public int Insert (T obj)
        {
            _objectSet.Add(obj);
            return Save();
        }

        //update methodu
        public int Update(T obj)
        {
            return Save();
        }

        //delete methodu
        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        //save methodu
        private int Save()
        {
            return db.SaveChanges();
        }

        //tek tip döndüren find 
        public T Find (Expression <Func<T,bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }


















    }
}

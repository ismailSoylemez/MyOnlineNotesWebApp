using MyOnlineNotes.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOnlineNotes.DataAccessLayer.EntityFramework
{
    //Test classı içinde repository den insert update delete yaparken her seferinde ayrı bir dbcontext oluşturuyordu ve bu bize karışıklığa neden oluyordu.
    //Singleton Repository : bir nesnenin program çalışınca yalnızca bir kopyası olsun ve herkes o kopyayı kullansın
    public class RepositoryBase
    {

        protected static DatabaseContext context;
        protected static object _locksync = new object();

        //constructor protecter ise bu class newlenemez
        protected RepositoryBase()
        {
           CreateContext();
        }

        //static method olduğu için newlenemeyecek..
        //bir kere newlenecek ve sğrekli onu kullanacak
        private static void  CreateContext()
        {
            if (context==null)
            {
                //aynı anda 2 thread çalışamaz
                //multithread uygulamalar için bile dbcontext 1 kere oluşacak
                lock (_locksync)
                {
                    context = new DatabaseContext();

                }
            }
        }



    }
}

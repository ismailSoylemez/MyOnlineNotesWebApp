using MyOnlineNotesEntities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOnlineNotes.BusinessLayer
{
    public class BusinessLayerResult<T> where T : class
    {

        public List<ErrorMessageObj> Errors { get; set; }//hata mesajını burada
        public T Result { get; set; }//sonucu burada


        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
            //error yoksa liste oluşmuyor.Bunu engellemek her koşulda bir listemin elimde bulunması gerekiyor
        }

        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObj() { Code= code , Message =message });
        }


    }
}

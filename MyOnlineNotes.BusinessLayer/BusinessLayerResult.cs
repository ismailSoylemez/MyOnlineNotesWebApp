using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOnlineNotes.BusinessLayer
{
    public class BusinessLayerResult<T> where T : class
    {

        public BusinessLayerResult()
        {
            Errors = new List<string>();//error yoksa liste oluşmuyor.Bunu engellemek her koşulda bir listemin elimde bulunması gerekiyor
        }

        public List<string>  Errors { get; set; }//hata mesajını burada
        public T Result { get; set; }//sonucu burada

    }
}

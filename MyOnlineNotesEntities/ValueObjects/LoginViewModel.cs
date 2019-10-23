using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


//UI İÇİNDE BULUNAN VİEWMODEL CLASSLARINI ENTİTY KATMANINA TAŞIDIK.ÇÜNKÜ KULLANICI KONTROLLERİNİ BUSİNESS KATMANINDA YAPACAĞIM BUSİNESS KATMANI UI KATMANINA DİREKT OLARAK ERİŞEMEZ.
namespace MyOnlineNotesEntities.ValueObject
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"),Required,StringLength(25)]
        public string Username { get; set; }

        [DisplayName("Şifre"), Required,DataType(DataType.Password),StringLength(25)]
        public string Password { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"),Required,StringLength(25)]
        public string Username { get; set; }

        [DisplayName("Şifre"), Required,DataType(DataType.Password),StringLength(25)]
        public string Password { get; set; }
    }
}
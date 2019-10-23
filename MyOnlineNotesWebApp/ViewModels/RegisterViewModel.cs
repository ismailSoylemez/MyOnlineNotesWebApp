using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyOnlineNotesWebApp.ViewModels
{
    public class RegisterViewModel
    {

        [DisplayName("Kullanıcı Adı"),Required,StringLength(25)]
        public string Username { get; set; }

        [DisplayName("Email"), Required, StringLength(70)]
        public string EMail { get; set; }

        [DisplayName("Şifre"), Required, StringLength(25)]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"), Required, StringLength(25),Compare("Password",ErrorMessage ="{0} ile {1} uyuşmuyor")]
        public string RePassword { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MyOnlineNotes.Common.Helpers
{
    public class ConfigHelper
    {
        public static T  Get<T> (string key)
        {

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));

        }



    }
}

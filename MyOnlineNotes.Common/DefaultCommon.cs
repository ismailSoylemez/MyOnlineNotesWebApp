using System;
using System.Collections.Generic;
using System.Text;

namespace MyOnlineNotes.Common
{
    public class DefaultCommon:ICommon
    {
        public string GetCurrentUsername()
        {
            return "system";
        }

    }
}

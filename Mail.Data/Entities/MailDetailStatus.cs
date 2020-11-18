using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Data.Entities
{
    public enum MailDetailStatus:int
    {
        Okunmadi = 0,
        Okundu = 1,
        Silinmedi = 2,
        Silindi = 3
    }
}

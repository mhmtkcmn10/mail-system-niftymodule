using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Repository
{
    public class DataConnection
    {
        public string Request(string data)
        {
            return data.ToUpper();
        }
    }
}

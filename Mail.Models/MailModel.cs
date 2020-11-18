using Cekirdek.Repository;
using MailApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Models
{
    public class MailModel : RepositoryModel
    {
        public Guid MailId { get; set; }
        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
        public MailDetailStatus MailStatus
        {
            get;
            set;
        }
        public Guid SenderID 
        {   
            get;
            set;
        }


        public UserLoginModel SenderInfo { get; set; }

        public List<MailDetailsModel> MailDetailsInfo { get; set; }



    }
}

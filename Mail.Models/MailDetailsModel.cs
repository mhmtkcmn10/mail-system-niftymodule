using Cekirdek.Repository;
using MailApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Models
{
    public class MailDetailsModel : RepositoryModel
    {
        public Guid MailDetailsId
        {
            get; 
            set;
        }
        public Guid MailParentID
        {
            get;
            set;
        }
        //public Guid UserSenderID
        //{
        //    get;
        //    set;
        //}

        public Guid UserTakerID
        {
            get;
            set;
        }
        
        public bool SenderStatus
        {
            get;
            set;
        }

        public MailDetailStatus mailDetailStatus
        {
            get;
            set;
        }

        //public bool MailReadStatus
        //{
        //    get;
        //    set;
        //}

        //public bool SenderDeleteStatus
        //{
        //    get;
        //    set;
        //}

        //public bool TakerDeleteStatus
        //{
        //    get;
        //    set;
        //}

        //public Status status { get; set; }


        public List<MailDetailsModel> MailsGroupBy { get; set; }
        public MailModel Mails { get; set; }
        public UserLoginModel UserSenderInfo { get; set; }
        public UserLoginModel UserTakerInfo { get; set; }
        public List<MailDetailsModel> AliciListesi { get; set; }




    }
}

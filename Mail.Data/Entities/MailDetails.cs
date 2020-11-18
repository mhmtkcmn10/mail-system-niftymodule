using Giga.Repository.MongoDB;
using MailApp.Data;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Data.Entities
{
    public class MailDetails : MongoDBEntity
    {
        public MailDetails()
        {

        }
        public MailDetails(MailDataContext context)
            : base(context)
        {

        }
        [BsonElement("mailParentID")]
        public Guid MailParentID
        {
            get;
            set;
        }
        //[BsonElement("userSenderID")]
        //public Guid UserSenderID
        //{
        //    get;
        //    set;
        //}
        [BsonElement("userTakerID")]
        public Guid UserTakerID
        {
            get;
            set;
        }

        [BsonElement("senderStatus")]
        public bool SenderStatus
        {
            get;
            set;
        }

        [BsonElement("mailDetailStatus")]
        public MailDetailStatus mailDetailStatus
        {
            get;
            set;
        }

        //[BsonElement("mailReadStatus")]
        //public bool MailReadStatus
        //{
        //    get;
        //    set;
        //}
        //[BsonElement("senderDeleteStatus")]
        //public bool SenderDeleteStatus
        //{
        //    get;
        //    set;
        //}
        //[BsonElement("TakerDeleteOprStatus")]
        //public bool TakerDeleteStatus
        //{
        //    get;
        //    set;
        //}

        //public Status status { get; set; }

    }
}

using Giga.Repository.MongoDB;
using MailApp.Data;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Data.Entities
{
    public class Mail : MongoDBEntity
    {
        public Mail()
        {

        }
        public Mail(MailDataContext context)
            : base(context)
        {

        }

        [BsonElement("title")]
        public string Title
        {
            get;
            set;
        }

        [BsonElement("message")]
        public string Message
        {
            get;
            set;
        }

        [BsonElement("date")]
        public DateTime Date
        {
            get;
            set;
        }

        [BsonElement("mailStatus")]
        public MailDetailStatus MailStatus
        {
            get;
            set;
        }

        [BsonElement("senderID")]
        public Guid SenderID
        {
            get;
            set;
        }


    }
}

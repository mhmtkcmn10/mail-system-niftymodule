using Giga.Repository.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailApp.Data.Entities
{
    public class UserLogin : MongoDBEntity
    {
        public UserLogin()
        {

        }

        public UserLogin(MailDataContext context)
            : base(context)
        {

        }

        [BsonElement("name")]
        public string Name
        {
            get;
            set;
        }

        [BsonElement("surname")]
        public string Surname
        {
            get;
            set;
        }
        [BsonElement("email")]
        public string Email
        {
            get;
            set;
        }

        [BsonElement("username")]
        public string Username
        {
            get;
            set;
        }

        [BsonElement("password")]
        public string Password
        {
            get;
            set;
        }

        [BsonElement("status")]
        public bool Status
        {
            get;
            set;
        }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenEndDate { get; set; }

    }
}
using Giga.Repository.MongoDB;
using MailApp.Data.Entities;
using System;
using System.Linq;

namespace MailApp.Data
{
    public class MailDataContext: MongoDBContext
    {
        public static string ConnectionString;
        public static string Database;
        public MailDataContext()
            :base(ConnectionString, Database)
        {

        }
        public IQueryable<UserLogin> User => MongoCollection<UserLogin>();

        public IQueryable<MailDetails> MailDetails => MongoCollection<MailDetails>();

        public IQueryable<Mail> Mail => MongoCollection<Mail>();






    }
}

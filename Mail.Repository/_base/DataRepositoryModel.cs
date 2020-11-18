using Cekirdek.RepositoryBase;
using Giga.Repository.MongoDB;
using MailApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Repository
{
    /// <summary>
    /// Veri Modeli üzerinde işlem sınıfı.
    /// </summary>
    /// <typeparam name="TEntity">Veri tabanı sorgulama sınıf tipi.</typeparam>
    /// <typeparam name="TModel">Dönüş sınıf tipi.</typeparam>
    /// <typeparam name="TVeriIslem">Veri işlem modeli sınıf tipi. Bu modelden türetilen sınıftır.</typeparam>
    public abstract class DataRepositoryBase<TEntity, TModel, TSelf> : DataRepositoryPattern<MailDataContext, Guid, TEntity, TModel, TSelf>, IDisposable
        //where TEntity : EntityNesnesi
        where TEntity : MongoDBEntity
        where TModel : class, new()
        where TSelf : DataRepositoryBase<TEntity, TModel, TSelf>, new()
    {
        static DataRepositoryBase()
        {
            Key = "ceFWgs82xKeN91fSQzYYA1Y20BUWQ5xrzpDtlZcZppK|2ow8p6Q77mvOHCLXLpLK59Uc|Eo|644v9rSFNnP4pD4bT_JsFUtXKxATeku7H05p8qQb_3MC_A==";

            //Kayıt belirteci (gerekli)
            Identifier = entity => entity.Id;
            //Guid belirtec (varsa)
            UID = entity => entity.Id;
            //Row versiyon eş zamanlı işlem çakışma kontrolü
            //RowVersion = entity => entity.TimeStamp;

            
            //Yeni context nasıl oluşturulacak
            NewDbContextExpression = accessKey => NewDbContext(accessKey);
            //Kayıt nasıl eklenecek
            InsertExpression = (entity, vm) => vm.Insert(entity);
            //Kayıt nasıl silinecek
            DeleteExpression = (entity, vm) => vm.Remove(entity);

            AccessControlFunction = (acKey, usr) =>
            {
                return true;
            };
        }

        private static MailDataContext NewDbContext(Guid accessKey)
        {
            var vm = new MailDataContext();
            return vm;
        }
    }
}
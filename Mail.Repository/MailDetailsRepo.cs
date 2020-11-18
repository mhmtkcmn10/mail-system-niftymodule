using MailApp.Data.Entities;
using MailApp.Models;
using MongoDB.Bson.IO;
using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace MailApp.Repository
{
    public class MailDetailsRepo : DataRepositoryBase<MailDetails, MailDetailsModel, MailDetailsRepo>
    {
        static MailDetailsRepo()
        {
            ModelIdentifier = mdl => mdl.MailDetailsId;

            QueryFunction = vm =>
                vm.MailDetails;

            #region Secme Fonksiyonları
            #region Ana Secme

            SelectFunction = (vm, qry, prms) =>
                from q in qry
                select new MailDetailsModel
                {
                    MailDetailsId = q.Id,
                    MailParentID = q.MailParentID,
                    UserTakerID = q.UserTakerID,
                    SenderStatus=q.SenderStatus,
                    mailDetailStatus=q.mailDetailStatus
                };
            #endregion

            #region Secme 
            //SelectFunctions[nameof(null)] = (vm, qry, prms) => null;

            SelectFunctions[nameof(GroupStack.Bilgi)] = (vm, qry, prms) =>
            from md in vm.MailDetails
            //let mp = vm.MailDetails.GroupBy(x=>x.MailParentID).ToList() //tablodaki join yerine let tanımlayarak liste yada tek veriyi çekmemize yarar
            select new MailDetailsModel
            {
                MailDetailsId = md.Id,
                MailParentID = md.MailParentID,
                UserTakerID = md.UserTakerID,
                SenderStatus = md.SenderStatus,
                mailDetailStatus = md.mailDetailStatus
            };

            #endregion

            #region Düzenleme fonksiyonları
            TransferFunction = trn =>
            {
                var mdl = trn.Model;
                var usr = trn.Entity;

                usr.MailParentID = mdl.MailParentID;
                usr.UserTakerID = mdl.UserTakerID;
                usr.SenderStatus = mdl.SenderStatus;
                usr.mailDetailStatus = mdl.mailDetailStatus;
            };
            #endregion

            #endregion
        }

        #region Filtreler
        public FilterStack Filter { get { return GetFilter<FilterStack>(); } }
        public class FilterStack : FilterBase
        {
            public MailDetailsRepo MailDetailsId(Guid mailid)
            {
                return Filter(q => q.Id == mailid);
            }
            public MailDetailsRepo MailParentsId(Guid mailParentid)
            {
                return Filter(q => q.MailParentID == mailParentid);
            }
            public MailDetailsRepo Takerid(Guid takerid)
            {
                return Filter(q => q.UserTakerID == takerid);
            }

            public MailDetailsRepo MailDetailStatusUnRead()
            {
                return Filter(q => q.mailDetailStatus == MailDetailStatus.Okunmadi);
            }
            public MailDetailsRepo MailDetailStatusDeleted()
            {
                return Filter(q => q.mailDetailStatus != MailDetailStatus.Silindi);
            }
            public MailDetailsRepo SenderStatus(bool senderStatus)
            {
                return Filter(q=>q.SenderStatus==senderStatus);
            }

        }
        #endregion

        #region Gruplar

        public GroupStack Group { get { return GetGroup<GroupStack>(); } }
        public class GroupStack : GroupBase
        {
            public MailDetailsRepo Bilgi()
            {
                return Group();
            }
            //Modify
            //Kayıtları döndüren join yerine tercih edilen verileri listeleme


            public MailDetailsRepo MailveUserBilgileriIle()
            {
                #region JoinIslemiYerineModifyKullanıyoruz
                //join mp in vm.MailParent
                //on md.Mailid equals mp.Id
                //join usr_1 in vm.User
                //on md.Senderid equals usr_1.Id
                //join usr_2 in vm.User
                //on md.Takerid equals usr_2.Id
                #endregion
                Modify(x =>
                {
                    x.Mails = MailRepo.NewInstance.Filter.Choice(x.MailParentID).Get.FirstOrDefault();
                    x.UserSenderInfo = UserLoginRepo.NewInstance.Filter.Choice(x.Mails.SenderID).Get.FirstOrDefault();
                    x.UserTakerInfo = UserLoginRepo.NewInstance.Filter.Choice(x.UserTakerID).Get.FirstOrDefault();
                });
                return Group();
            }
        }
        #endregion

        //#region Siralamalar
        //public BolumDersIslem Siralama()
        //{
        //    return addorder(qry =>
        //        qry.O(q => q.));
        //}
        //#endregion

        #region Fonksiyonlar
        //İşlemler bu sınıf altında tanımlanmalı
        public ToDoStack Do { get { return GetToDo<ToDoStack>(); } }
        public class ToDoStack : ToDoBase
        {
            //public Process Function()
            //{
            //    var p = CreateProcess(RepositoryProcessType.Update);
            //    return p;
            //}
        }
        #endregion

        #region Fonksiyonlar as parallel
        //İşlemler bu sınıf altında tanımlanmalı
        //async işlem yapmamızı sağlar (Parallel)
        public ToDoAsParallelStack DoAsParallel { get { return GetToDoAsParallel<ToDoAsParallelStack>(); } }
        public class ToDoAsParallelStack : ToDoAsParallelBase
        {

        }
        #endregion

        #region Ek Islemler
        #endregion
    }
}

using MailApp.Data.Entities;
using MailApp.Models;
using System;
using System.Linq;

namespace MailApp.Repository
{
    public class MailRepo : DataRepositoryBase<Mail, MailModel, MailRepo>
    {
        static MailRepo()
        {
            ModelIdentifier = mdl => mdl.MailId;

            QueryFunction = vm =>
                vm.Mail;

            #region Secme Fonksiyonları
            #region Ana Secme


            SelectFunction = (vm, qry, prms) =>
                from q in qry
                select new MailModel
                {
                    MailId = q.Id,
                    Title = q.Title,
                    Message = q.Message,
                    Date = q.Date,
                    MailStatus=q.MailStatus,
                    SenderID = q.SenderID
                };

            SelectFunctions[nameof(GroupStack.Bilgi)] = (vm, qry, prms) =>
            from mm in vm.Mail
            //join md in vm.MailDetails
            //on mm.Id equals md.MailParentID
            //join usr_2 in vm.User
            //on md.UserTakerID equals usr_2.Id
            select new MailModel
            {
                MailId = mm.Id,
                Title = mm.Title,
                Message = mm.Message,
                Date = mm.Date,
                SenderID=mm.SenderID
            };

            #endregion

            #endregion

            #region Düzenleme fonksiyonları
            TransferFunction = trn =>
            {
                var mdl = trn.Model;
                var usr = trn.Entity;

                usr.Title = mdl.Title;
                usr.Message = mdl.Message;
                usr.Date = mdl.Date;
                usr.MailStatus = mdl.MailStatus;
                usr.SenderID = mdl.SenderID;
            };
            #endregion
        }

        #region Filtreler
        public FilterStack Filter { get { return GetFilter<FilterStack>(); } }
        public class FilterStack : FilterBase
        {
            public MailRepo MailID(Guid mailid)
            {
                return Filter(q => q.Id == mailid);
            }

            public MailRepo Title(string title)
            {
                return Filter(q => q.Title == title);
            }
            public MailRepo Message(string message)
            {
                return Filter(q => q.Message == message);
            }
            public MailRepo MailStatusSilindiEsitOlmayan()
            {
                return Filter(q => q.MailStatus != MailDetailStatus.Silindi);
            }

            public MailRepo SenderID(Guid senderid)
            {
                return Filter(q => q.SenderID == senderid);
            }
        }
        #endregion

        #region Gruplar

        public GroupStack Group { get { return GetGroup<GroupStack>(); } }
        public class GroupStack : GroupBase
        {
            public MailRepo Bilgi()
            {
                return Group();
            }

            public MailRepo UserBilgileriIle()
            {
                Modify(x =>
                {
                    x.SenderInfo = UserLoginRepo.NewInstance.Filter.Choice(x.SenderID).Get.FirstOrDefault();
                    x.MailDetailsInfo = MailDetailsRepo.NewInstance.Filter.MailParentsId(x.MailId)
                                                                    .Filter.SenderStatus(false)
                                                                    .Group.Bilgi()
                                                                    .Group.MailveUserBilgileriIle()
                                                                    .Get.ToList();
                });
                return Group();
            }
        }
        #endregion

        #region Siralamalar
        //public BolumDersIslem Siralama()
        //{
        //    return addorder(qry =>
        //        qry.orderby(q => q.));
        //}

        #endregion

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

using MailApp.Data.Entities;
using MailApp.Models;
using System.Linq;

namespace MailApp.Repository
{
    public class UserLoginRepo : DataRepositoryBase<UserLogin, UserLoginModel, UserLoginRepo>
    {
        static UserLoginRepo()
        {
            ModelIdentifier = mdl => mdl.UserId;

            QueryFunction = vm =>
                vm.User;

            #region Secme Fonksiyonları
            #region Ana Secme

            SelectFunction = (vm, qry, prms) =>
                from q in qry
                select new UserLoginModel
                {
                    UserId = q.Id,
                    Name = q.Name,
                    Surname = q.Surname,
                    Email = q.Email,
                    Username = q.Username,
                    Password = q.Password,
                    Status = q.Status,
                };
            #endregion

            #region Secme 
            //SelectFunctions[nameof(null)] = (vm, qry, prms) => null;
            #endregion
            #endregion

            #region Düzenleme fonksiyonları
            TransferFunction = trn =>
            {
                var mdl = trn.Model;
                var usr = trn.Entity;

                usr.Name = mdl.Name;
                usr.Surname = mdl.Surname;
                usr.Email = mdl.Email;
                usr.Username = mdl.Username;
                usr.Password = mdl.Password;
                usr.Status = mdl.Status;
                usr.RefreshToken = mdl.RefreshToken;
                usr.RefreshTokenEndDate = mdl.RefreshTokenEndDate;
            };
            #endregion
        }

        #region Filtreler
        public FilterStack Filter { get { return GetFilter<FilterStack>(); } }
        public class FilterStack : FilterBase
        {
            public UserLoginRepo Name(string name)
            {
                return Filter(q => q.Name == name);
            }
            public UserLoginRepo Surname(string surname)
            {
                return Filter(q => q.Surname == surname);
            }
            public UserLoginRepo Email(string email)
            {
                return Filter(q => q.Email == email);
            }
            public UserLoginRepo UserName(string userName)
            {
                return Filter(q => q.Username == userName);
            }
            public UserLoginRepo Password(string password)
            {
                return Filter(q => q.Password == password);
            }
            public UserLoginRepo RefreshToken(string refreshToken)
            {
                return Filter(q => q.RefreshToken == refreshToken);
            }
            public UserLoginRepo UserRegister(string email, string username)
            {
                return Filter(q => q.Email == email || q.Username == username);
            }
            public UserLoginRepo UserLogin(string username, string password)
            {
                return Filter(q => q.Username == username || q.Password == password);
            }
        }
        #endregion

        #region Gruplar

        public GroupStack Group { get { return GetGroup<GroupStack>(); } }
        public class GroupStack : GroupBase
        {
            public UserLoginRepo Bilgi()
            {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using MailApp.Data.Entities;
using MailApp.Models;
using MailApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using X.PagedList;

namespace mailNiftymebis_netcore.Controllers
{
    [Authorize]
    public class MBInboxController : Controller
    {
        //public IActionResult GetPartial()
        //{
        //    List<string> countries = new List<string>();
        //    countries.Add("USA");
        //    countries.Add("ENG");
        //    return PartialView("_CountriesPartial", countries);
        //}

        public IActionResult MBInbox(string Sorting_Order, int? Page_No)/*int page = 1, int pageSize = 10*/
        {
            var usrEmail = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                  .Select(c => c.Value)
                                                  .SingleOrDefault();
            var userTakerId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                                     .Select(c => c.Value)
                                                     .SingleOrDefault();
            ViewBag.userName_SurName = HttpContext.User.Identity.Name.ToString();
            ViewBag.userEmail = usrEmail;

            var modelDetail = MailDetailsRepo.NewInstance.Filter.Takerid(Guid.Parse(userTakerId))
                                                         .Filter.MailDetailStatusDeleted()
                                                         .Filter.SenderStatus(false)    
                                                         .Group.Bilgi()
                                                         .Group.MailveUserBilgileriIle()
                                                         .Get.ToList();

            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingDate = Sorting_Order == null || Sorting_Order == "near_date" ? "far_date" : "near_date";

            if (!string.IsNullOrEmpty(Sorting_Order))
                modelDetail = modelDetail.OrderBy(x => x.Mails.Date).ToList();
            else if (Sorting_Order == "near_date")
                modelDetail = modelDetail.OrderBy(x => x.Mails.Date).ToList();
            else
                modelDetail = modelDetail.OrderByDescending(x => x.Mails.Date).ToList();

            var mailReadInfoCount = MailDetailsRepo.NewInstance.Filter.Takerid(new Guid(userTakerId))
                                                               .Filter.SenderStatus(false)
                                                               .Filter.MailDetailStatusUnRead()
                                                               .Get.ToList().Count();
            ViewBag.readCount = mailReadInfoCount;

            int Size_Of_Page = 5;
            int No_Of_Page = (Page_No ?? 1);

            PagedList<MailDetailsModel> detailsModel = new PagedList<MailDetailsModel>(modelDetail, No_Of_Page, Size_Of_Page);
            ViewBag.PagedListMailDetails = detailsModel;
            return View(detailsModel);
        }
        [HttpGet]
        public IActionResult MBInboxMes(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var usrEmail = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                      .Select(c => c.Value)
                                                      .SingleOrDefault();

                var userTakerId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                                         .Select(c => c.Value)
                                                         .SingleOrDefault();

                ViewBag.userName_SurName = HttpContext.User.Identity.Name.ToString();
                ViewBag.userEmail = usrEmail;

                ViewBag.loginList = UserLoginRepo.NewInstance.Get.ToList();

                MailDetailsModel currentAccount = MailDetailsRepo.NewInstance.Filter.MailDetailsId(Guid.Parse(id))
                                                                              .Get.FirstOrDefault();

                var modelDetail = MailDetailsRepo.NewInstance.Group.Bilgi()
                                                              .Group.MailveUserBilgileriIle()
                                                              .Get.ToList();

                modelDetail = modelDetail.Where(x => x.UserTakerID == Guid.Parse(userTakerId) && x.MailDetailsId == Guid.Parse(id))
                                                                    .OrderByDescending(x => x.Mails.Date).ToList();
                ViewBag.mailbilgi = modelDetail;

                var mailReadInfoCount = MailDetailsRepo.NewInstance.Filter.Takerid(new Guid(userTakerId))
                                                                   .Filter.SenderStatus(false)
                                                                   .Filter.MailDetailStatusUnRead()
                                                                   .Get.ToList().Count();
                ViewBag.readCount = mailReadInfoCount;
                if (currentAccount.mailDetailStatus == MailDetailStatus.Okunmadi)
                {
                    currentAccount.mailDetailStatus = MailDetailStatus.Okundu;
                    var test = MailDetailsRepo.NewInstance.Do.Update(currentAccount);
                }
                return View("MBInboxMES");
            }
        }
        public void DataDelete(string id)
        {
            var currentAccount = MailDetailsRepo.NewInstance.Filter.MailDetailsId(Guid.Parse(id)).Get.FirstOrDefault();
            currentAccount.mailDetailStatus = MailDetailStatus.Silindi;
            var test = MailDetailsRepo.NewInstance.Do.Update(currentAccount);
        }
        [HttpPost]
        public JsonResult MBInboxMesMailMessageDelete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    DataDelete(id);
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (ArgumentNullException ne)
            {
                ne.ToString();
                return Json(new { Success = false });
            }
        }
        [HttpPost]
        public JsonResult MBInboxMailDelete(string[] deletedItems)
        {
            try
            {
                if (!string.IsNullOrEmpty(deletedItems.ToString()))
                {
                    foreach (string inboxID in deletedItems)
                    {
                        DataDelete(inboxID);
                    }
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (ArgumentNullException ne)
            {
                ne.ToString();
                return Json(new { Success = false });
            }
        }
        [HttpPost]
        public JsonResult MailUnRead(string[] updatedItems)
        {
            try
            {
                if (!string.IsNullOrEmpty(updatedItems.ToString()))
                {
                    foreach (string inboxID in updatedItems)
                    {
                        var currentAccount = MailDetailsRepo.NewInstance.Filter.MailDetailsId(Guid.Parse(inboxID)).Get.FirstOrDefault();
                        currentAccount.mailDetailStatus = MailDetailStatus.Okunmadi;
                        var test = MailDetailsRepo.NewInstance.Do.Update(currentAccount);
                    }
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (ArgumentNullException ne)
            {
                ne.ToString();
                return Json(new { Success = false });
            }
        }
        [HttpPost]
        public JsonResult MailRead(string[] updatedItems)
        {
            try
            {
                if (!string.IsNullOrEmpty(updatedItems.ToString()))
                {
                    foreach (string inboxID in updatedItems)
                    {
                        var currentAccount = MailDetailsRepo.NewInstance.Filter.MailDetailsId(Guid.Parse(inboxID)).Get.FirstOrDefault();
                        currentAccount.mailDetailStatus = MailDetailStatus.Okundu;
                        var test = MailDetailsRepo.NewInstance.Do.Update(currentAccount);
                    }
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (ArgumentNullException ne)
            {
                ne.ToString();
                return Json(new { result = false });
            }
        }
        [HttpPost]
        public JsonResult MailInboxCompose(string title, string message, string[] taker)
        {
            MailModel mailParent = new MailModel();
            MailDetailsModel mailDetails = new MailDetailsModel();

            if (string.IsNullOrEmpty(taker.ToString()) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(title))
            {
                return Json(new { result = false });
            }
            else
            {
                var userSenderId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                                           .Select(c => c.Value)
                                                           .SingleOrDefault();

                mailParent.Title = title;
                mailParent.Message = message;
                mailParent.Date = DateTime.Now;
                mailParent.SenderID = Guid.Parse(userSenderId);
                mailParent.MailStatus = MailDetailStatus.Silinmedi;
                var mailParentMessageInsert = MailRepo.NewInstance.Do.Insert(mailParent);

                mailDetails.MailParentID = Guid.Parse(mailParentMessageInsert.ID.ToString());
                mailDetails.UserTakerID = Guid.Parse(userSenderId);
                mailDetails.SenderStatus = true;
                mailDetails.mailDetailStatus = MailDetailStatus.Okunmadi;

                var mailDetailsSenderFirstInsert = MailDetailsRepo.NewInstance.Do.Insert(mailDetails);

                mailDetails.MailParentID = Guid.Parse(mailParentMessageInsert.ID.ToString());
                mailDetails.mailDetailStatus = MailDetailStatus.Okunmadi;
                var mailIDzero = new Guid();

                foreach (var item in taker)
                {
                    mailDetails.UserTakerID = Guid.Parse(item);
                    mailDetails.SenderStatus = false;
                    var test = MailDetailsRepo.NewInstance.Do.Insert(mailDetails);
                    mailDetails.MailDetailsId = mailIDzero;
                }
                return Json(new { result = true });
            }
        }

    }
}

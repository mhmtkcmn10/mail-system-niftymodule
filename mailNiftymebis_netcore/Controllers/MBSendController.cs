using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class MBSendController : Controller
    {
        public IActionResult MBSend(string Sorting_Order, int? Page_No)
        {
            var usrEmail = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                  .Select(c => c.Value)
                                                  .SingleOrDefault();
            
            var userTakerId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                                     .Select(c => c.Value)
                                                     .SingleOrDefault();

            ViewBag.userName_SurName = HttpContext.User.Identity.Name.ToString();
            ViewBag.userEmail = usrEmail;

            var modelDetail = MailRepo.NewInstance.Filter.SenderID(Guid.Parse(userTakerId))
                                                    .Filter.MailStatusSilindiEsitOlmayan()
                                                    .Group.Bilgi()
                                                    .Group.UserBilgileriIle()
                                                    .Get.ToList();

            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingDate = Sorting_Order == null || Sorting_Order == "near_date" ? "far_date" : "near_date";

            if (!string.IsNullOrEmpty(Sorting_Order))
                modelDetail = modelDetail.OrderBy(x => x.Date).ToList();
            else if (Sorting_Order == "near_date")
                modelDetail = modelDetail.OrderBy(x => x.Date).ToList();
            else
                modelDetail = modelDetail.OrderByDescending(x => x.Date).ToList();

            var mailReadInfoCount = MailDetailsRepo.NewInstance.Filter.Takerid(new Guid(userTakerId))
                                                               .Filter.SenderStatus(false)
                                                               .Filter.MailDetailStatusUnRead()
                                                               .Get.ToList().Count();
            ViewBag.readCount = mailReadInfoCount; 
            int Size_Of_Page = 5;
            int No_Of_Page = (Page_No ?? 1);
            PagedList<MailModel> detailsModel = new PagedList<MailModel>(modelDetail, No_Of_Page, Size_Of_Page);
            return View(detailsModel);
        }

        [HttpGet]
        public IActionResult MBSendMES(string id)
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
                ViewBag.userInformationList = UserLoginRepo.NewInstance.Get.ToList();

                var mailInfoList = MailRepo.NewInstance.Filter.MailID(Guid.Parse(id))
                                                       .Filter.MailStatusSilindiEsitOlmayan()
                                                       .Group.Bilgi()
                                                       .Group.UserBilgileriIle()
                                                       .Get.ToList();
                ViewBag.MailInfoList = mailInfoList;

                var mailReadInfoCount = MailDetailsRepo.NewInstance.Filter.Takerid(new Guid(userTakerId))
                                                                   .Filter.SenderStatus(false)
                                                                   .Filter.MailDetailStatusUnRead()
                                                                   .Get.ToList().Count();
                ViewBag.readCount = mailReadInfoCount;

                return View("MBSendMES");

            }
        }

        public void DataDelete(string id)
        {
            List<MailModel> currentAccount = MailRepo.NewInstance.Filter.MailID(Guid.Parse(id)).Get.ToList();
            var mailDetailsList = currentAccount;

            foreach (var item in mailDetailsList)
            {
                item.MailStatus = MailDetailStatus.Silindi;
                var updateSenderDeleteStatus = MailRepo.NewInstance.Do.Update(item);
            }
        }

        [HttpPost]
        public JsonResult MBSendMesMailMessageDelete(string id)
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
        public JsonResult MBSendMailDelete(string[] deletedItems)
        {
            try
            {
                if (!string.IsNullOrEmpty(deletedItems.ToString()))
                {
                    foreach (string id in deletedItems)
                    {
                        DataDelete(id);
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
        public JsonResult MailSendCompose(string title, string message, string[] taker)
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

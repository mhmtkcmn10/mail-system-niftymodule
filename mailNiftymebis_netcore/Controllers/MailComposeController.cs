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
    public class MailComposeController : Controller
    {
        public void UserInfoListele()
        {
            var result = UserLoginRepo.NewInstance.Get.ToList();
            ViewBag.userInformationList = result;
        }

        public IActionResult MailCompose()
        {
            UserInfoListele();

            var usrEmail = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)
               .Select(c => c.Value).SingleOrDefault();

            var userId = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value).SingleOrDefault();

            ViewBag.userName_SurName = HttpContext.User.Identity.Name.ToString();
            ViewBag.userEmail = usrEmail;

            var mailReadInfoCount = MailDetailsRepo.NewInstance.Filter.Takerid(new Guid(userId)).Filter.SenderStatus(false).Filter.MailDetailStatusUnRead().Get.ToList().Count();
            ViewBag.readCount = mailReadInfoCount;

            return View();
        }

        [HttpPost]
        public IActionResult MailCompose(string title, string message, string[] taker)
        {
            MailModel mailParent = new MailModel();
            MailDetailsModel mailDetails = new MailDetailsModel();

            if (string.IsNullOrEmpty(taker.ToString()) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(title))
            {
                UserInfoListele();
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
                UserInfoListele();
                return Json(new { result = true });
            }
        }
    }
}

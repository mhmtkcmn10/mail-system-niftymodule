using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailApp.Models;
using MailApp.Repository;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace mailNiftymebis_netcore.Controllers
{
    public class UserRegisterController : Controller
    {
        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserRegister(UserLoginModel userLogin)
        {
            if (userLogin == null)
            {
                return Json(new { Success =false});
            }
            else
            {
                var userObje = UserLoginRepo.NewInstance.Filter.UserRegister(userLogin.Email, userLogin.Username)
                                                        .Get.IsExist();

                if (userObje)
                {
                    return Json(new { result = false });
                }
                else
                {
                    userLogin.Password = BC.HashPassword(userLogin.Password);

                    var ekleIslem = UserLoginRepo.NewInstance.Do.Insert(userLogin);
                    
                    return Json(new { result=ekleIslem.Successful });
                }
            }
        }
    }
}

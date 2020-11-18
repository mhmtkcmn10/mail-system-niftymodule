using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MailApp.Data;
using MailApp.Models;
using MailApp.Repository;
using mailNiftymebis_netcore.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;


namespace mailNiftymebis_netcore.Controllers
{
    public class UserLoginController : Controller
    {
        readonly IConfiguration configuration;

        public UserLoginController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserLogin(string username,string password)
        {
            var userLogin = UserLoginRepo.NewInstance.Filter.UserLogin(username, password).Get.FirstOrDefault();

            if (userLogin == null || !BC.Verify(password, userLogin.Password))
            {
                return Json(new { result = false });
            }
            else
            {
                //Token üretiliyor.
                TokenHandler tokenHandler = new TokenHandler(configuration);
                Token.Token token = tokenHandler.CreateAccessToken(userLogin);

                //Refresh token Users tablosuna işleniyor.
                userLogin.RefreshToken = token.RefreshToken;
                userLogin.RefreshTokenEndDate = token.Expiration.AddMinutes(3);

                var updateRefresh = UserLoginRepo.NewInstance.Do.Update(userLogin);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid,userLogin.UserId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,userLogin.RefreshToken),
                    new Claim(ClaimTypes.Name,userLogin.Name+" "+userLogin.Surname),
                    new Claim(ClaimTypes.Email,userLogin.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                return Json(new { result = true });
            }
        }

        [HttpPost]
        public JsonResult RefreshToken()
        {
            var userRefreshToken = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault();

            var userLogin = UserLoginRepo.NewInstance.Filter.RefreshToken(userRefreshToken).Get.FirstOrDefault();

            if (userLogin != null)/*&& user?.RefreshTokenEndDate > DateTime.Now*/
            {
                TokenHandler tokenHandler = new TokenHandler(configuration);
                Token.Token token = tokenHandler.CreateAccessToken(userLogin);
                userLogin.RefreshToken = token.RefreshToken;
                userLogin.RefreshTokenEndDate = token.Expiration.AddMinutes(3);

                var updateRefresh = UserLoginRepo.NewInstance.Do.Update(userLogin);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid,userLogin.UserId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,userLogin.RefreshToken),
                    new Claim(ClaimTypes.Name,userLogin.Name+" "+userLogin.Surname),
                    new Claim(ClaimTypes.Email,userLogin.Email),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }
    }
}


﻿using MailApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mailNiftymebis_netcore.Token
{
    public class TokenHandler
    {
        public IConfiguration Configuration { get; set; }
        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Token CreateAccessToken(UserLoginModel userLogin)
        {
            Token tokenInstance = new Token();
            //Security  Key'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Oluşturulacak olan token expiration süresini belirliyoruz.
            tokenInstance.Expiration = DateTime.Now.AddMinutes(5);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: Configuration["Token:Issuer"],
                audience: Configuration["Token:Audience"],
                expires: tokenInstance.Expiration,//Token süresini 5 dk olarak belirliyorum
                notBefore: DateTime.Now,    //Token üretildikten ne kadar süre sonra+ devreye girsin ayarlıyouz.
                signingCredentials: signingCredentials
                );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); //Token oluşturucu sınıfında bir örnek alıyoruz.
            tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken); //Token üretiyoruz.
            tokenInstance.RefreshToken = CreateRefreshToken(); //Refresh Token üretiyoruz.
            return tokenInstance;
        }
        //Refrest Token Oluşturuluyor.
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}

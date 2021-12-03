using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Core.Authentication.Api.Models;

namespace WebAPI.Core.Authentication.Api.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        #region Properties
        private List<User> users = new List<User>() {
            new User { UserId = 1, FirstName = "Sohan", UserName = "Sohan", Password ="Sohan@123" },
            new User { UserId = 2, FirstName = "Sumit", UserName = "Sumit", Password ="Sumit@123" },
            new User { UserId = 3, FirstName = "Eswar", UserName = "Eswar", Password ="Eswar@123" },
            new User { UserId = 4, FirstName = "Arun", UserName = "Arun", Password ="Arun@123" },
            new User { UserId = 5, FirstName = "Melishya", UserName = "Melishya", Password ="Melishya@123" }
        };
        //private readonly AppSettings _appSettings;
        private readonly string _secret;
        private readonly string _expDate;
        #endregion
        public AuthenticateService(/*IOptions<AppSettings> appSettings, */IConfiguration config)
        {
            //_appSettings = appSettings.Value; 
            _secret = config.GetSection("JwtConfig").GetSection("Secret").Value;
            _expDate = config.GetSection("JwtConfig").GetSection("ExpirationInMilisecond").Value;
        }

        public async Task<User> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            var user = users.SingleOrDefault(x => x.UserName == userName && x.Password == password);

            // return null if usernot valid
            if (user == null)
                return null;

            //User Found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Version, "V3.1")
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }
    }
}

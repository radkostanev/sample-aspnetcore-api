using SampleNetCoreApi.Api.Auth.Contracts;
using SampleNetCoreApi.Api.Helpers;
using SampleNetCoreApi.Api.Models;
using SampleNetCoreApi.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SampleNetCoreApi.Api.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings appSettings;
        private List<User> users = new List<User>
        {
            new User 
            { 
                Id = 1, FirstName = "Bruce", 
                LastName = "Banner", 
                Username = "bruce", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("banner") 
            }
        };

        public AuthService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public AuthenticateResponse Register(RegisterRequest model)
        {
            if (model.Password != model.ConfirmPassword 
                || this.users.Any(user => user.Username == model.Username))
            {
                throw new Exception();
            }

            var user = new User 
            { 
                FirstName = model.FirstName, 
                LastName = model.LastName, 
                Username = model.Username, 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) 
            };

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = users.SingleOrDefault(u => u.Username == model.Username 
            && BCrypt.Net.BCrypt.Verify(model.Password, u.PasswordHash));

            if (user == null)
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll() => this.users;

        public User GetById(int id) => this.users.FirstOrDefault(User => User.Id == id);

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

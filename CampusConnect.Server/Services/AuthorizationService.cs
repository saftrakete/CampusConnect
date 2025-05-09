using CampusConnect.Server.Data;
using CampusConnect.Server.Interfaces;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CampusConnect.Server.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _config;

        public AuthorizationService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Authorizes the user and returns a JwtToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Authorize(UserModel user, LoginDto loginDto)
        {
            var passwordHasher = new PasswordHasher<UserModel>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            return verificationResult == PasswordVerificationResult.Success;
        }

        public string GenerateJwtToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.LoginName),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), //1h Gültigkeit fürs Erste
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

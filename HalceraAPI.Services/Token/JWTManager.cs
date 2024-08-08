using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Token
{
    public static class JWTManager
    {

        /// <summary>
        /// Create Json Web Token for user
        /// </summary>
        /// <param name="applicationUser">User from Db</param>
        /// <returns>Json Token</returns>
        public static string CreateToken(ApplicationUser applicationUser, string jwtSecretToken)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, applicationUser.Name ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, applicationUser.Email)
            };

            if (applicationUser.Roles != null && applicationUser.Roles.Any())
            {
                foreach (var role in applicationUser.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Title));
                }
            }

            // key to create and verify JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretToken));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(29),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}

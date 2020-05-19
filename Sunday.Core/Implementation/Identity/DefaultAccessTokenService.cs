using Microsoft.IdentityModel.Tokens;
using Sunday.Core.Application.Identity;
using Sunday.Core.Domain.Users;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Sunday.Core.Implementation.Identity
{
    [ServiceTypeOf(typeof(IAccessTokenService))]
    public class DefaultAccessTokenService : IAccessTokenService
    {
        public string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ApplicationSettings.Get("Sunday.JwtSecret") ?? "d0125f4126c5447bb801994521e78675");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

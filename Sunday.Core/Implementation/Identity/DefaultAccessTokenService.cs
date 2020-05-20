using Microsoft.IdentityModel.Tokens;
using Sunday.Core.Application.Identity;
using Sunday.Core.Domain.Users;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Principal;

namespace Sunday.Core.Implementation.Identity
{
    [ServiceTypeOf(typeof(IAccessTokenService))]
    public class DefaultAccessTokenService : IAccessTokenService
    {
        private const string DefaultSecret = "d0125f4126c5447bb801994521e78675";
        public virtual string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetSecret());
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

        public virtual bool ValidToken(string token, out int? userId)
        {
            userId = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetSecret());
            var validateParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validateParameters, out var securityToken);
                var identity = principal.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated)
                    return false;
                var userIdClaim = identity.FindFirst(ClaimTypes.Name);
                string strUserId = userIdClaim?.Value;
                if (string.IsNullOrEmpty(strUserId) || !int.TryParse(strUserId, out int tmpUserId))
                    return false;
                userId = tmpUserId;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected virtual string GetSecret()
        {
            return ApplicationSettings.Get("Sunday.JwtSecret") ?? DefaultSecret;
        }
    }
}

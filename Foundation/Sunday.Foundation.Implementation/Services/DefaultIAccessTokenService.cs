using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Sunday.Core;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IAccessTokenService))]
    public class DefaultIAccessTokenService : IAccessTokenService
    {
        private const string DefaultSecret = "d0125f4126c5447bb801994521e78675";
        public string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetSecret());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidToken(string token, out Guid? userId)
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
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validateParameters, out var securityToken);
            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
                return false;
            var userIdClaim = identity.FindFirst(ClaimTypes.Name);
            string? strUserId = userIdClaim?.Value;
            if (string.IsNullOrEmpty(strUserId) || !Guid.TryParse(strUserId, out Guid tmpUserId))
                return false;
            userId = tmpUserId;
            return true;
        }
        protected virtual string GetSecret()
        => ApplicationSettings.Get("Sunday.JwtSecret") ?? DefaultSecret;
    }
}

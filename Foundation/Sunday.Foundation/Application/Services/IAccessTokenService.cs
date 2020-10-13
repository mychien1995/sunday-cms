using System;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface IAccessTokenService
    {
        string GenerateToken(ApplicationUser user);

        bool ValidToken(string token, out Guid? userId);
    }
}

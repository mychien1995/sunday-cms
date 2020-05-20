using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Application.Identity
{
    public interface IAccessTokenService
    {
        string GenerateToken(ApplicationUser user);

        bool ValidToken(string token, out int? userId);
    }
}

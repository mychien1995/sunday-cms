using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Identity.Application
{
    public interface IAccessTokenService
    {
        string GenerateToken(IApplicationUser user);

        bool ValidToken(string token, out int? userId);
    }
}

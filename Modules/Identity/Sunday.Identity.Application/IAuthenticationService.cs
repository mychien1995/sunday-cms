using Sunday.Core.Domain.Users;
using Sunday.Identity.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Identity.Application
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult<IApplicationUser>> AuthenticateAsync(string username, string password, bool loginToShell = false, bool remember = false);
    }
}

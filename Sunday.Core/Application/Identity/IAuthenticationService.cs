using Sunday.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Application.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(string username, string password, bool loginToShell = false, bool remember = false);
    }
}

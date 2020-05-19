﻿using Sunday.CMS.Core.Application.Identity;
using Sunday.CMS.Core.Models;
using Sunday.Core;
using Sunday.Core.Application.Identity;
using Sunday.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Identity
{
    [ServiceTypeOf(typeof(ILoginRepository))]
    public class LoginRepository : ILoginRepository
    {
        private readonly IAuthenticationService _authenticationService;
        public LoginRepository(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<LoginApiResponse> LoginAsync(LoginInputModel credential)
        {
            var result = new LoginApiResponse();
            result.Success = false;
            var authenticateResult = await _authenticationService.AuthenticateAsync(credential.Username, credential.Password, true);
            switch (authenticateResult.SignInResult)
            {
                case LoginStatus.Success:
                    result.Success = true;
                    break;
                case LoginStatus.LockedOut:
                    result.AddError("Your account has been locked out");
                    break;
                case LoginStatus.RequiresVerification:
                    result.AddError("Your account has not been verified");
                    break;
                case LoginStatus.Failure:
                default:
                    result.AddError("Invalid username or passsword");
                    break;
            }
            if (authenticateResult.SignInResult != LoginStatus.Success) return result;
            result.Email = authenticateResult.User.Email;
            result.Fullname = authenticateResult.User.Fullname;
            result.Phone = authenticateResult.User.Phone;
            result.Username = credential.Username;
            result.Token = authenticateResult.AccessToken;
            return result;
        }
    }
}

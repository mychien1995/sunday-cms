using Sunday.CMS.Core.Application.Identity;
using Sunday.CMS.Core.Application.Organizations;
using Sunday.CMS.Core.Models;
using Sunday.Core;
using Sunday.Core.Media.Application;
using Sunday.Identity.Application;
using Sunday.Identity.Core;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Identity
{
    [ServiceTypeOf(typeof(ILoginManager))]
    public class LoginManager : ILoginManager
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IBlobLinkManager _blobLinkManager;
        private readonly IOrganizationAccessManager _accessManager;
        public LoginManager(IAuthenticationService authenticationService, IBlobLinkManager blobLinkManager, IOrganizationAccessManager accessManager)
        {
            _authenticationService = authenticationService;
            _blobLinkManager = blobLinkManager;
            _accessManager = accessManager;
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
            if (!_accessManager.AllowAccess(authenticateResult.User))
            {
                result.AddError("You are not allowed to access this hostname");
                return result;
            }
            result.Email = authenticateResult.User.Email;
            result.Fullname = authenticateResult.User.Fullname;
            result.Phone = authenticateResult.User.Phone;
            result.Username = credential.Username;
            result.Token = authenticateResult.AccessToken;
            result.AvatarLink = _blobLinkManager.GetPreviewLink(authenticateResult.User.AvatarBlobUri, true);
            return result;
        }
    }
}

using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Login;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Media.Application;
using Sunday.Foundation;
using Sunday.Foundation.Application.Services;

namespace Sunday.CMS.Core.Implementation
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
            var result = new LoginApiResponse { Success = false };
            var authenticateResult = await _authenticationService.PasswordSignInAsync(credential.Username, credential.Password, true);
            switch (authenticateResult.Status)
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
                    result.AddError("Invalid username or password");
                    break;
            }
            if (authenticateResult.Status != LoginStatus.Success) return result;
            var user = authenticateResult.User.Get();
            if (!_accessManager.AllowAccess(user))
            {
                result.AddError("You are not allowed to access this hostname");
                return result;
            }
            result.Email = user.Email;
            result.Fullname = user.Fullname;
            result.Phone = user.Phone;
            result.Username = credential.Username;
            result.Token = authenticateResult.AccessToken;
            if (user.AvatarBlobUri != null)
                result.AvatarLink = _blobLinkManager.GetPreviewLink(user.AvatarBlobUri, true);
            return result;
        }
    }
}

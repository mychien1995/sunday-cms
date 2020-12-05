using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Models.Users.JsonResults;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IProfileManager))]
    public class DefaultProfileManager : IProfileManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public DefaultProfileManager(IHttpContextAccessor httpContextAccessor, IApplicationUserManager applicationUserManager,
            IUserService userService, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicationUserManager = applicationUserManager;
            _userService = userService;
            _identityService = identityService;
        }

        public async Task<UserProfileJsonResult> GetCurrentUserProfile()
        {
            var currentUserPrincipal = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            var currentUserReponse = await _applicationUserManager.GetUserById(currentUserPrincipal!.UserId);
            return currentUserReponse.MapTo<UserProfileJsonResult>();
        }

        public async Task<BaseApiResponse> UpdateProfile(UserMutationModel mutationModel)
        {
            var currentUserPrincipal = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            mutationModel.Id = currentUserPrincipal!.UserId;
            var currentUser = await _userService.GetUserByIdAsync(currentUserPrincipal.UserId).MapResultTo(u => u.Get());
            mutationModel.IsActive = currentUser.IsActive;
            mutationModel.UserName = currentUser.UserName;
            mutationModel.AvatarBlobUri = currentUser.AvatarBlobUri!;
            mutationModel.RoleIds = currentUser.Roles.Select(r => r.Id).ToList();
            mutationModel.Domain = currentUser.Domain;
            var result = await _applicationUserManager.UpdateUser(mutationModel);
            return result;
        }

        public async Task<BaseApiResponse> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var result = new BaseApiResponse();
            var currentUserPrincipal = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            var changePwdResult = await _identityService.ChangePasswordAsync(currentUserPrincipal!.UserId,
                changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            result.Success = changePwdResult;
            return result;
        }

        public async Task<BaseApiResponse> ChangeAvatar(ChangeAvatarModel changeAvatarModel)
        {
            var currentUserPrincipal = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            var currentUser = await _userService.GetUserByIdAsync(currentUserPrincipal!.UserId).MapResultTo(u => u.Get());
            currentUser.AvatarBlobUri = changeAvatarModel.BlobIdentifier; await _userService.UpdateAsync(currentUser);
            return BaseApiResponse.SuccessResult;
        }
    }
}

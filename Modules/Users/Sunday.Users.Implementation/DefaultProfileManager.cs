using Microsoft.AspNetCore.Http;
using Sunday.Core;
using Sunday.Identity.Application;
using Sunday.Identity.Core;
using Sunday.Users.Application;
using Sunday.Users.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation
{
    [ServiceTypeOf(typeof(IProfileManager))]
    public class DefaultProfileManager : IProfileManager
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        public DefaultProfileManager(IHttpContextAccessor httpContextAccessor, IApplicationUserManager applicationUserManager
            , IUserRepository userRepository, IIdentityService identityService)
        {
            _httpContextAccesor = httpContextAccessor;
            _userRepository = userRepository;
            _applicationUserManager = applicationUserManager;
            _identityService = identityService;
        }
        public async virtual Task<UserProfileJsonResult> GetCurrentUserProfile()
        {
            var currentUserPrincipal = _httpContextAccesor.HttpContext.User as ApplicationUserPrincipal;
            var currentUserReponse = await _applicationUserManager.GetUserById(currentUserPrincipal.UserId);
            return currentUserReponse.MapTo<UserProfileJsonResult>();
        }

        public async virtual Task<BaseApiResponse> UpdateProfile(UserMutationModel mutationModel)
        {
            var currentUserPrincipal = _httpContextAccesor.HttpContext.User as ApplicationUserPrincipal;
            mutationModel.ID = currentUserPrincipal.UserId;
            var currentUser = _userRepository.GetUserById(currentUserPrincipal.UserId);
            mutationModel.IsActive = currentUser.IsActive;
            mutationModel.UserName = currentUser.UserName;
            mutationModel.AvatarBlobUri = currentUser.AvatarBlobUri;
            mutationModel.RoleIds = currentUser.Roles.Select(x => x.ID).ToList();
            mutationModel.Domain = currentUser.Domain;
            var result = await _applicationUserManager.UpdateUser(mutationModel);
            return result;
        }

        public async virtual Task<BaseApiResponse> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var result = new BaseApiResponse();
            var currentUserPrincipal = _httpContextAccesor.HttpContext.User as ApplicationUserPrincipal;
            var changePwdResut = await _identityService.ChangePasswordAsync(currentUserPrincipal.UserId,
                changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            result.Success = changePwdResut;
            return result;
        }
        public async virtual Task<BaseApiResponse> ChangeAvatar(ChangeAvatarModel changeAvatarModel)
        {
            var currentUserPrincipal = _httpContextAccesor.HttpContext.User as ApplicationUserPrincipal;
            var currentUser = _userRepository.GetUserById(currentUserPrincipal.UserId);
            var result = await _userRepository.UpdateAvatar(currentUser.ID, changeAvatarModel.BlobIdentifier);
            return new BaseApiResponse();
        }
    }
}

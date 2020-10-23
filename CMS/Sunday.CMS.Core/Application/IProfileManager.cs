using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Models.Users.JsonResults;
using Sunday.Core.Models.Base;
using Sunday.Users.Core.Models;

namespace Sunday.CMS.Core.Application
{
    public interface IProfileManager
    {
        Task<UserProfileJsonResult> GetCurrentUserProfile();

        Task<BaseApiResponse> UpdateProfile(UserMutationModel mutationModel);

        Task<BaseApiResponse> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<BaseApiResponse> ChangeAvatar(ChangeAvatarModel changeAvatarModel);
    }
}

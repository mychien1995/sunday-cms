using Sunday.Core;
using Sunday.Users.Core.Models;
using System.Threading.Tasks;

namespace Sunday.Users.Application
{
    public interface IProfileManager
    {
        Task<UserProfileJsonResult> GetCurrentUserProfile();

        Task<BaseApiResponse> UpdateProfile(UserMutationModel mutationModel);

        Task<BaseApiResponse> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<BaseApiResponse> ChangeAvatar(ChangeAvatarModel changeAvatarModel);
    }
}

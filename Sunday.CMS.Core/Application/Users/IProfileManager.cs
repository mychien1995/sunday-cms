using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Users
{
    public interface IProfileManager
    {
        Task<UserProfileJsonResult> GetCurrentUserProfile();

        Task<BaseApiResponse> UpdateProfile(UserMutationModel mutationModel);

        Task<BaseApiResponse> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<BaseApiResponse> ChangeAvatar(ChangeAvatarModel changeAvatarModel);
    }
}

using Sunday.CMS.Core.Models;
using Sunday.CMS.Core.Models.Users;
using Sunday.Core.Domain.Users;
using Sunday.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Application.Users
{
    public interface IApplicationUserManager
    {
        Task<UserListJsonResult> SearchUsers(SearchUserCriteria criteria);

        Task<CreateUserJsonResult> CreateUser(UserMutationModel userData);

        Task<UpdateUserJsonResult> UpdateUser(UserMutationModel userData);

        Task<UserDetailJsonResult> GetUserById(int userId);

        Task<BaseApiResponse> DeleteUser(int userId);

        Task<BaseApiResponse> ActivateUser(int userId);

        Task<BaseApiResponse> DeactivateUser(int userId);

        Task<BaseApiResponse> ResetUserPassword(int userId);
    }
}

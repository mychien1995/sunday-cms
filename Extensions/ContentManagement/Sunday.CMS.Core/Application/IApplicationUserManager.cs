using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Models.Users.JsonResults;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationUserManager
    {
        Task<UserListJsonResult> SearchUsers(UserQuery criteria);

        Task<CreateUserJsonResult> CreateUser(UserMutationModel userData);

        Task<UpdateUserJsonResult> UpdateUser(UserMutationModel userData);

        Task<UserDetailJsonResult> GetUserById(Guid userId);

        Task<BaseApiResponse> DeleteUser(Guid userId);

        Task<BaseApiResponse> ActivateUser(Guid userId);

        Task<BaseApiResponse> DeactivateUser(Guid userId);

        Task<BaseApiResponse> ResetUserPassword(Guid userId);
    }
}

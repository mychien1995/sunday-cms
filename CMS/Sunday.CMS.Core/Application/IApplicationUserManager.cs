using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Users;
using Sunday.CMS.Core.Models.Users.JsonResults;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;
using Sunday.Users.Core.Models;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationUserManager
    {
        Task<UserListJsonResult> SearchUsers(UserQuery criteria);

        Task<CreateUserJsonResult> CreateUser(UserMutationModel userData);

        Task<UpdateUserJsonResult> UpdateUser(UserMutationModel userData);

        Task<UserDetailJsonResult> GetUserById(int userId);

        Task<BaseApiResponse> DeleteUser(int userId);

        Task<BaseApiResponse> ActivateUser(int userId);

        Task<BaseApiResponse> DeactivateUser(int userId);

        Task<BaseApiResponse> ResetUserPassword(int userId);
    }
}

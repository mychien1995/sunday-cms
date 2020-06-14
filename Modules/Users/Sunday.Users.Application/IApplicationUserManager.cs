using Sunday.Core;
using Sunday.Users.Core.Models;
using System.Threading.Tasks;

namespace Sunday.Users.Application
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

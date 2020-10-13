using Sunday.Core.Models;
using Sunday.Users.Core;
using Sunday.Users.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Models.Base;

namespace Sunday.Users.Application
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindUserByNameAsync(string username);
        Task<ApplicationUser> FindUserByEmailAsync(string email);

        Task<SearchResult<ApplicationUser>> QueryUsers(UserQuery query);

        ApplicationUser GetUserById(int userId);

        Task<ApplicationUser> CreateUser(ApplicationUser user);

        Task<ApplicationUser> UpdateUser(ApplicationUser user);

        Task<ApplicationUser> UpdateAvatar(int userId, string blobIdentifier);

        Task<bool> DeleteUser(int userId);

        Task FetchUserRoles(List<ApplicationUser> users);

        Task<bool> ActivateUser(int userId);

        Task<bool> DeactivateUser(int userId);

        Task<bool> UpdatePassword(ApplicationUser user);

        Task FetchVirtualRoles(List<ApplicationUser> users);
    }
}

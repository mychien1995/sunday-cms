using Sunday.Core.Domain.Users;
using Sunday.Core.Models;
using Sunday.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Users
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindUserByNameAsync(string username);

        Task<SearchResult<ApplicationUser>> QueryUsers(UserQuery query);

        ApplicationUser GetUserById(int userId);

        ApplicationUser GetUserWithOptions(int userId, GetUserOptions option = null);

        Task<ApplicationUser> CreateUser(ApplicationUser user);

        Task<ApplicationUser> UpdateUser(ApplicationUser user);

        Task<bool> DeleteUser(int userId);
    }
}

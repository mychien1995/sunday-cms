using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Users
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindUserByNameAsync(string username);
    }
}

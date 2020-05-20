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
    }
}

using Sunday.Core.Domain.Roles;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Users.Application
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();

        Task<ApplicationRole> GetRoleById(int roleId);

        Task<ApplicationRole> GetRoleByCode(string roleCode);
    }
}

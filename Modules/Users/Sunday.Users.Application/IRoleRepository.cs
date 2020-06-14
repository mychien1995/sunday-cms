using Sunday.Users.Core;
using System.Collections.Generic;
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

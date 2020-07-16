using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;

namespace Sunday.Foundation.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleEntity>> GetAllAsync();

        Task<RoleEntity> GetRoleByIdAsync(int roleId);

        Task<RoleEntity> GetRoleByCodeAsync(string roleCode);
    }
}

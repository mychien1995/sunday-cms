using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleEntity>> GetAllAsync();

        Task<Option<RoleEntity>> GetRoleByIdAsync(int roleId);

        Task<Option<RoleEntity>> GetRoleByCodeAsync(string roleCode);
    }
}

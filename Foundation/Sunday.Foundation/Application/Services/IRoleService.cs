using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRole>> GetAllAsync();

        Task<Option<ApplicationRole>> GetRoleByIdAsync(int roleId);

        Task<Option<ApplicationRole>> GetRoleByCodeAsync(string roleCode);
    }
}

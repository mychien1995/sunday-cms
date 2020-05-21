using Sunday.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
    }
}

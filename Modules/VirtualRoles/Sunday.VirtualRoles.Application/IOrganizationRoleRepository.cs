using Sunday.Core.Models;
using Sunday.VirtualRoles.Core;
using Sunday.VirtualRoles.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunday.VirtualRoles.Application
{
    public interface IOrganizationRoleRepository
    {
        Task<ISearchResult<OrganizationRole>> GetRoles(OrganizationRoleQuery query);
        Task<OrganizationRole> GetById(int organizationRoleId);
        Task<int> Create(OrganizationRole role);
        Task<OrganizationRole> Update(OrganizationRole role);
        Task<bool> Delete(int roleId);
        Task<bool> BulkUpdate(IEnumerable<OrganizationRole> roles);
    }
}

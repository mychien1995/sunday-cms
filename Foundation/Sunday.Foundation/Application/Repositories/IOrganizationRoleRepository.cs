using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Models;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Repositories
{
    public interface IOrganizationRoleRepository
    {
        Task<ISearchResult<OrganizationRoleEntity>> QueryAsync(OrganizationRoleQuery query);
        Task<OrganizationRoleEntity> GetRoleByIdAsync(int organizationRoleId);
        Task<int> CreateAsync(OrganizationRoleEntity role);
        Task<OrganizationRoleEntity> UpdateAsync(OrganizationRoleEntity role);
        Task<bool> DeleteAsync(int roleId);
        Task<bool> BulkUpdateAsync(IEnumerable<OrganizationRoleEntity> roles);
    }
}

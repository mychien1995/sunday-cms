using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Application.Repositories
{
    public interface IOrganizationRoleRepository
    {
        Task<SearchResult<OrganizationRoleEntity>> QueryAsync(OrganizationRoleQuery query);
        Task<Option<OrganizationRoleEntity>> GetRoleByIdAsync(Guid organizationRoleId);
        Task<Guid> CreateAsync(OrganizationRoleEntity role);
        Task UpdateAsync(OrganizationRoleEntity role);
        Task DeleteAsync(Guid roleId);
        Task BulkUpdateAsync(IEnumerable<OrganizationRoleEntity> roles);
    }
}

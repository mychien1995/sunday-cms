using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Services
{
    public interface IOrganizationRoleService
    {
        Task<SearchResult<ApplicationOrganizationRole>> QueryAsync(OrganizationRoleQuery query);
        Task<Option<ApplicationOrganizationRole>> GetRoleByIdAsync(Guid organizationRoleId);
        Task<Guid> CreateAsync(ApplicationOrganizationRole role);
        Task UpdateAsync(ApplicationOrganizationRole role);
        Task DeleteAsync(Guid roleId);
        Task BulkUpdateAsync(IEnumerable<ApplicationOrganizationRole> roles);
    }
}

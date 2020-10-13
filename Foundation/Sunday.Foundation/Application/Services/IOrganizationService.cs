using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Services
{
    public interface IOrganizationService
    {
        Task<SearchResult<ApplicationOrganization>> QueryAsync(OrganizationQuery query);

        Task<Option<ApplicationOrganization>> GetOrganizationByIdAsync(Guid organizationId);


        Task<Guid> CreateAsync(ApplicationOrganization organization);

        Task UpdateAsync(ApplicationOrganization organization);

        Task DeleteAsync(Guid organizationId);

        Task ActivateAsync(Guid organizationId);

        Task DeactivateAsync(Guid organizationId);

        Task<Option<ApplicationOrganization>> FindOrganizationByHostname(string hostName);
    }
}

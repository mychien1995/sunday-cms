using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Models;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Application.Repositories
{
    public interface IOrganizationRepository
    {
        Task<SearchResult<OrganizationEntity>> QueryAsync(OrganizationQuery query);

        Task<Option<OrganizationEntity>> GetOrganizationByIdAsync(Guid organizationId);


        Task<Guid> CreateAsync(OrganizationEntity organization);

        Task UpdateAsync(OrganizationEntity organization);

        Task DeleteAsync(Guid organizationId);

        Task ActivateAsync(Guid organizationId);

        Task DeactivateAsync(Guid organizationId);
    }
}

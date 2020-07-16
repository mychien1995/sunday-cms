using System.Threading.Tasks;
using Sunday.Core.Models;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Implementation.Repositories.Entities;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Application.Repositories
{
    public interface IOrganizationRepository
    {
        Task<SearchResult<OrganizationEntity>> QueryAsync(OrganizationQuery query);

        Task<OrganizationEntity> GetOrganizationByIdAsync(int organizationId);


        Task<OrganizationEntity> CreateAsync(OrganizationEntity organization);

        Task<OrganizationEntity> UpdateAsync(OrganizationEntity organization);

        Task<bool> DeleteAsync(int organizationId);

        Task<bool> ActivateAsync(int organizationId);

        Task<bool> DeactivateAsync(int organizationId);
    }
}

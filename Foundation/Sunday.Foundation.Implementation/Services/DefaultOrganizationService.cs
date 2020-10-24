using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;
using static LanguageExt.Prelude;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IOrganizationService))]
    public class DefaultOrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public DefaultOrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public Task<SearchResult<ApplicationOrganization>> QueryAsync(OrganizationQuery query)
            => _organizationRepository.QueryAsync(query).MapResultTo(rs => rs.CloneTo<ApplicationOrganization>());

        public Task<Option<ApplicationOrganization>> GetOrganizationByIdAsync(Guid organizationId)
            => _organizationRepository.GetOrganizationByIdAsync(organizationId)
                .MapResultTo(org => org.Map(o => o.MapTo<ApplicationOrganization>()));

        public Task<Guid> CreateAsync(ApplicationOrganization organization)
            => _organizationRepository.CreateAsync(organization.MapTo<OrganizationEntity>());

        public Task UpdateAsync(ApplicationOrganization organization)
            => _organizationRepository.UpdateAsync(organization.MapTo<OrganizationEntity>());

        public Task DeleteAsync(Guid organizationId)
            => _organizationRepository.DeleteAsync(organizationId);

        public Task ActivateAsync(Guid organizationId)
            => _organizationRepository.ActivateAsync(organizationId);

        public Task DeactivateAsync(Guid organizationId)
            => _organizationRepository.DeactivateAsync(organizationId);

        public async Task<Option<ApplicationOrganization>> FindOrganizationByHostname(string hostName)
        {
            var organizations = await _organizationRepository.QueryAsync(new OrganizationQuery()
            {
                HostNames = new[] {hostName}
            });
            return Optional(organizations.Result.FirstOrDefault()).Map(ToDomainModel);
        }

        private static ApplicationOrganization ToDomainModel(OrganizationEntity entity)
        {
            return entity.MapTo<ApplicationOrganization>();
        }
    }
}

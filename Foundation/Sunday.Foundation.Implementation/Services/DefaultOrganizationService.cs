using System;
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

        public Task<Option<ApplicationOrganization>> FindOrganizationByHostname(string hostName)
        {
            throw new NotImplementedException();
        }
    }
}

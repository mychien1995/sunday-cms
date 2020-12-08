using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Newtonsoft.Json;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Extensions;
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
            => _organizationRepository.QueryAsync(query).MapResultTo(rs => new SearchResult<ApplicationOrganization>(rs.Total,
                rs.Result.Select(ToDomainModel).ToArray()));

        public Task<Option<ApplicationOrganization>> GetOrganizationByIdAsync(Guid organizationId)
            => _organizationRepository.GetOrganizationByIdAsync(organizationId)
                .MapResultTo(org => org.Map(ToDomainModel));

        public Task<Guid> CreateAsync(ApplicationOrganization organization)
            => _organizationRepository.CreateAsync(ToEntity(organization));

        public Task UpdateAsync(ApplicationOrganization organization)
            => _organizationRepository.UpdateAsync(ToEntity(organization));

        public Task DeleteAsync(Guid organizationId)
            => _organizationRepository.DeleteAsync(organizationId);

        public Task ActivateAsync(Guid organizationId)
            => _organizationRepository.ActivateAsync(organizationId);

        public Task DeactivateAsync(Guid organizationId)
            => _organizationRepository.DeactivateAsync(organizationId);

        public async Task<List<ApplicationOrganization>> FindOrganizationByHostname(string hostName)
        {
            var organizations = await _organizationRepository.QueryAsync(new OrganizationQuery()
            {
                HostName = hostName
            });
            return organizations.Result.Select(ToDomainModel).ToList();
        }

        private static ApplicationOrganization ToDomainModel(OrganizationEntity entity)
        {
            var model = entity.MapTo<ApplicationOrganization>();
            model.Modules = entity.Modules.CastListTo<ApplicationModule>().ToList();
            model.HostNames = entity.Hosts != null ? entity.Hosts.ToStringList() : new List<string>();
            model.Properties = entity.ExtraProperties != null
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(entity.ExtraProperties)
                : new Dictionary<string, object>();
            return model;
        }
        private static OrganizationEntity ToEntity(ApplicationOrganization model)
        {
            var entity = model.MapTo<OrganizationEntity>();
            entity.Modules = model.Modules.CastListTo<ModuleEntity>().ToList();
            entity.Hosts = model.HostNames.ToDatabaseList();
            entity.ExtraProperties = model.Properties.Any() ? JsonConvert.SerializeObject(model.Properties) : null;
            return entity;
        }
    }
}

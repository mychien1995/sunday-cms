using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IOrganizationRoleService))]
    public class DefaultOrganizationRoleService : IOrganizationRoleService
    {
        private readonly IOrganizationRoleRepository _organizationRoleRepository;
        private readonly ISundayContext _sundayContext;

        public DefaultOrganizationRoleService(IOrganizationRoleRepository organizationRoleRepository, ISundayContext sundayContext)
        {
            _organizationRoleRepository = organizationRoleRepository;
            _sundayContext = sundayContext;
        }

        public Task<SearchResult<ApplicationOrganizationRole>> QueryAsync(OrganizationRoleQuery query)
          => _organizationRoleRepository.QueryAsync(EnsureQuery(query)).MapResultTo(rs => new SearchResult<ApplicationOrganizationRole>
          {
              Result = rs.Result.Select(ToDomainModel).ToList(),
              Total = rs.Total
          });

        public Task<Option<ApplicationOrganizationRole>> GetRoleByIdAsync(Guid organizationRoleId)
            => _organizationRoleRepository.GetRoleByIdAsync(organizationRoleId)
                .MapResultTo(org => org.Map(ToDomainModel));

        public async Task<Guid> CreateAsync(ApplicationOrganizationRole role)
        {
            role.Id = Guid.NewGuid();
            EnsureData(role);
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(role));
            return await _organizationRoleRepository.CreateAsync(ToEntity(role));
        }

        public async Task UpdateAsync(ApplicationOrganizationRole role)
        {
            EnsureData(role);
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(role));
            await _organizationRoleRepository.UpdateAsync(ToEntity(role));
        }

        public Task DeleteAsync(Guid roleId)
            => _organizationRoleRepository.DeleteAsync(roleId);

        public Task BulkUpdateAsync(IEnumerable<ApplicationOrganizationRole> roles)
            => _organizationRoleRepository.BulkUpdateAsync(roles.CastListTo<OrganizationRoleEntity>());

        private static OrganizationRoleEntity ToEntity(ApplicationOrganizationRole model)
        {
            var role = model.MapTo<OrganizationRoleEntity>();
            role.Features = model.Features.CastListTo<FeatureEntity>().ToList();
            return role;
        }

        private static ApplicationOrganizationRole ToDomainModel(OrganizationRoleEntity roleEntity)
        {
            var model = roleEntity.MapTo<ApplicationOrganizationRole>();
            model.Features = roleEntity.Features.CastListTo<ApplicationFeature>().ToList();
            return model;
        }

        private OrganizationRoleQuery EnsureQuery(OrganizationRoleQuery query)
        {
            if ((_sundayContext.CurrentUser!.IsOrganizationMember()))
                query.OrganizationId = _sundayContext.CurrentOrganization!.Id;
            return query;
        }

        private void EnsureData(ApplicationOrganizationRole role)
        {
            if ((_sundayContext.CurrentUser!.IsOrganizationMember()))
                role.OrganizationId = _sundayContext.CurrentOrganization!.Id;
        }
    }
}

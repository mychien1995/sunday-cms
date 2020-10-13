using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Implementation.Services
{
    public class DefaultOrganizationRoleService : IOrganizationRoleService
    {
        private readonly IOrganizationRoleRepository _organizationRoleRepository;

        public DefaultOrganizationRoleService(IOrganizationRoleRepository organizationRoleRepository)
        {
            _organizationRoleRepository = organizationRoleRepository;
        }

        public Task<SearchResult<ApplicationOrganizationRole>> QueryAsync(OrganizationRoleQuery query)
          => _organizationRoleRepository.QueryAsync(query).MapResultTo(rs => rs.CloneTo<ApplicationOrganizationRole>());

        public Task<Option<ApplicationOrganizationRole>> GetRoleByIdAsync(Guid organizationRoleId)
            => _organizationRoleRepository.GetRoleByIdAsync(organizationRoleId)
                .MapResultTo(org => org.Map(o => o.MapTo<ApplicationOrganizationRole>()));

        public Task<Guid> CreateAsync(ApplicationOrganizationRole role)
            => _organizationRoleRepository.CreateAsync(role.MapTo<OrganizationRoleEntity>());

        public Task UpdateAsync(ApplicationOrganizationRole role)
            => _organizationRoleRepository.UpdateAsync(role.MapTo<OrganizationRoleEntity>());

        public Task DeleteAsync(Guid roleId)
            => _organizationRoleRepository.DeleteAsync(roleId);

        public Task BulkUpdateAsync(IEnumerable<ApplicationOrganizationRole> roles)
            => _organizationRoleRepository.BulkUpdateAsync(roles.CastListTo<OrganizationRoleEntity>());
    }
}

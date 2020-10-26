using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Persistence.Application.Repositories;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IRoleService))]
    public class DefaultRoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public DefaultRoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<ApplicationRole>> GetAllAsync()
            => _roleRepository.GetAllAsync().MapResultTo(rs => rs.CastListTo<ApplicationRole>());

        public Task<Option<ApplicationRole>> GetRoleByIdAsync(Guid roleId)
            => _roleRepository.GetRoleByIdAsync(roleId)
                .MapResultTo(rs => rs.Map(role => role.MapTo<ApplicationRole>()));

        public Task<Option<ApplicationRole>> GetRoleByCodeAsync(string roleCode)
            => _roleRepository.GetRoleByCodeAsync(roleCode)
                .MapResultTo(rs => rs.Map(role => role.MapTo<ApplicationRole>()));
    }
}

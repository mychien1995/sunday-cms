using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Constants;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Core.Ultilities;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Implementation.Pipelines.Arguments;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IUserService))]
    public class DefaultUserService : IUserService
    {
        private readonly ISundayContext _sundayContext;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public DefaultUserService(IUserRepository userRepository, ISundayContext sundayContext, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _sundayContext = sundayContext;
            _roleRepository = roleRepository;
        }

        public Task<SearchResult<ApplicationUser>> QueryAsync(UserQuery query)
            => _userRepository.QueryAsync(query).MapResultTo(rs => new SearchResult<ApplicationUser>(rs.Total,
                rs.Result.Select(ToDomainModel).ToArray()));

        public Task<Option<ApplicationUser>> GetUserByIdAsync(Guid userId)
            => _userRepository.GetUserByIdAsync(userId)
                .MapResultTo(org => org.Map(ToDomainModel));

        public async Task<Guid> CreateAsync(ApplicationUser user)
        {
            await ApplicationPipelines.RunAsync("cms.users.beforeCreate", new BeforeCreateUserArg(user));
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(user));
            var entity = ToEntity(user);
            HashPassword(entity, user.Password);
            return await _userRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await ApplicationPipelines.RunAsync("cms.users.beforeUpdate", new BeforeUpdateUserArg(user));
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(user));
            await _userRepository.UpdateAsync(ToEntity(user));
        }

        public Task DeleteAsync(Guid userId)
            => _userRepository.DeleteAsync(userId);

        public Task ActivateAsync(Guid userId)
            => _userRepository.ActivateAsync(userId);

        public Task DeactivateAsync(Guid userId)
            => _userRepository.DeactivateAsync(userId);

        public Task UpdatePasswordAsync(ApplicationUser user)
            => _userRepository.UpdatePasswordAsync(user.MapTo<UserEntity>());

        private ApplicationUser ToDomainModel(UserEntity entity)
        {
            var user = entity.MapTo<ApplicationUser>();
            user.VirtualRoles = entity.VirtualRoles.CastListTo<ApplicationOrganizationRole>().ToList();
            user.Roles = entity.Roles.CastListTo<ApplicationRole>().ToList();
            user.OrganizationUsers = entity.OrganizationUsers.Select(u =>
            {
                var model = u.MapTo<ApplicationOrganizationUser>();
                model.Organization = new ApplicationOrganization(u.OrganizationId, u.OrganizationName);
                return model;
            }).ToList();
            return user;
        }

        private static void HashPassword(UserEntity user, string password)
        {
            var securityHash = Guid.NewGuid().ToString("N");
            var passwordHash = EncryptUltis.Sha256Encrypt(password, securityHash);
            user.PasswordHash = passwordHash;
            user.SecurityStamp = securityHash;
        }

        private UserEntity ToEntity(ApplicationUser user)
        {
            var entity = user.MapTo<UserEntity>();
            var currentUser = _sundayContext.CurrentUser!;
            entity.VirtualRoles = user.VirtualRoles.CastListTo<OrganizationRoleEntity>().ToList();
            entity.Domain = currentUser.Domain;
            entity.Roles = user.Roles.CastListTo<RoleEntity>().ToList();
            if (currentUser.IsInRole(SystemRoleCodes.OrganizationAdmin) || currentUser.IsInRole(SystemRoleCodes.OrganizationUser))
            {
                entity.OrganizationUsers = user.OrganizationUsers.Select(u =>
                {
                    var orgUserEntity = u.MapTo<OrganizationUserEntity>();
                    orgUserEntity.OrganizationId = _sundayContext.CurrentOrganization!.Id;
                    return orgUserEntity;
                }).ToList();
                if (!entity.OrganizationUsers.Any())
                {
                    entity.OrganizationUsers = new List<OrganizationUserEntity>
                        { new OrganizationUserEntity{OrganizationId =_sundayContext.CurrentOrganization!.Id, IsActive = true}};
                }
                entity.Roles = new List<RoleEntity>()
                    {_roleRepository.GetRoleByCodeAsync(SystemRoleCodes.OrganizationUser).Result.Get()};
            }
            return entity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.DataAccess.SqlServer.Extensions;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;
using Sunday.Foundation.Persistence.Implementation.DapperParameters;

namespace Sunday.Foundation.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IUserRepository))]
    public class DefaultUserRepository : IUserRepository
    {
        private readonly StoredProcedureRunner _dbRunner;

        public DefaultUserRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public async Task<SearchResult<UserEntity>> QueryAsync(UserQuery query)
        {
            var result = new SearchResult<UserEntity>();
            var returnTypes = new List<Type>() { typeof(int), typeof(UserEntity) };
            if (query.IncludeRoles) returnTypes.Add(typeof(UserRoleEntity));
            if (query.IncludeOrganizations) returnTypes.Add(typeof(OrganizationUserEntity));
            if (query.IncludeVirtualRoles) returnTypes.Add(typeof(OrganizationUserRoleEntity));
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Users.Search, returnTypes, DbQuery(query));
            result.Total = (int)searchResult[0].Single();
            var users = searchResult[1].Select(u => (UserEntity)u).ToList();
            var roles = new List<UserRoleEntity>();
            var organizationUsers = new List<OrganizationUserEntity>();
            var virtualRoles = new List<OrganizationUserRoleEntity>();
            if (query.IncludeRoles)
                roles = searchResult[2].Select(r => (UserRoleEntity)r).ToList();
            if (query.IncludeOrganizations)
                organizationUsers = searchResult[query.IncludeRoles ? 3 : 2].Select(r => (OrganizationUserEntity)r).ToList();
            if (query.IncludeVirtualRoles)
                virtualRoles = searchResult[query.IncludeRoles ? query.IncludeOrganizations ? 4 : 3 : query.IncludeOrganizations ? 3 : 2].Select(r => (OrganizationUserRoleEntity)r).ToList();
            users.Iter(user =>
            {
                user.Roles = roles.Where(ur => ur.UserId == user.Id)
                    .Select(ur => new RoleEntity(ur.RoleId, ur.Code, ur.RoleName)).ToList();
                user.OrganizationUsers = organizationUsers.Where(ou => ou.UserId == user.Id).ToList();
                user.VirtualRoles = virtualRoles.Select(or =>
                        (Role: or, User: user.OrganizationUsers.FirstOrDefault(ou => ou.Id == or.OrganizationUserId)))
                    .Where(or => or.User != null)
                    .Select(or =>
                        new OrganizationRoleEntity(or.Role.OrganizationRoleId, or.User.OrganizationId,
                            or.Role.RoleName))
                    .ToList();
            });
            result.Result = users;
            return result;
        }

        //TODO: optimize this
        public async Task<Option<UserEntity>> GetUserByIdAsync(Guid userId)
        {
            var queryResult =
                await _dbRunner
                    .ExecuteMultipleAsync<UserEntity, RoleEntity, OrganizationUserEntity, OrganizationRoleEntity>(
                        ProcedureNames.Users.GetByIdWithOptions, new { UserId = userId });
            if (!queryResult.Item1.Any()) return Option<UserEntity>.None;
            var user = queryResult.Item1.Single();
            user.Roles = queryResult.Item2.ToList();
            user.OrganizationUsers = queryResult.Item3.ToList();
            user.VirtualRoles = queryResult.Item4.ToList();
            return user;
        }

        public async Task<Guid> CreateAsync(UserEntity user)
        {
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            await _dbRunner.ExecuteAsync(ProcedureNames.Users.Insert, new CreateUserDynamicParameter(user));
            return user.Id;
        }

        public Task UpdateAsync(UserEntity user)
            => _dbRunner.ExecuteAsync(ProcedureNames.Users.Update, new UpdateUserDynamicParameter(user));

        public Task DeleteAsync(Guid userId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Users.Delete, new { UserId = userId });

        public Task ActivateAsync(Guid userId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Users.Activate, new { UserId = userId });

        public Task DeactivateAsync(Guid userId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Users.Deactivate, new { UserId = userId });

        public Task UpdatePasswordAsync(UserEntity user)
        => _dbRunner.ExecuteAsync(ProcedureNames.Users.ChangePassword, new
        {
            UserId = user.Id,
            SecurityHash = user.SecurityStamp,
            user.PasswordHash
        });

        public async Task<Option<UserEntity>> FindUserByNameAsync(string userName)
        {
            var queryResult =
                await _dbRunner
                    .ExecuteMultipleAsync<UserEntity, RoleEntity>(
                        ProcedureNames.Users.FindUserByUserName, new { Username = userName }); if (!queryResult.Item1.Any()) return Option<UserEntity>.None;
            var user = queryResult.Item1.Single();
            user.Roles = queryResult.Item2.ToList();
            return user;
        }

        private static object DbQuery(UserQuery query)
        => new
        {
            query.Username,
            query.Email,
            ExcludeIds = query.ExcludeIdList.ToDatabaseList(),
            IncludeIds = query.IncludeIdList.ToDatabaseList(),
            OrganizationIds = query.OrganizationIds.ToDatabaseList(),
            query.SortBy,
            query.Text,
            query.PageIndex,
            query.PageSize,
            query.IncludeRoles,
            query.IncludeVirtualRoles,
            query.IncludeOrganizations
        };
    }
}

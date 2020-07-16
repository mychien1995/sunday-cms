using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.VirtualRoles;
using Sunday.Core.Models;
using Sunday.DataAccess.SqlServer;
using Sunday.Foundation.Application.Repositories;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;
using Sunday.Foundation.Models;

namespace Sunday.Foundation.Implementation.Repositories
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
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Users.Search, new[] { typeof(int), typeof(UserEntity) }, query);
            result.Total = searchResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = searchResult[1].Select(x => (UserEntity)x);
            return result;
        }

        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            var returnTypes = new List<Type>() { typeof(UserEntity), typeof(RoleEntity), typeof(OrganizationUserEntity), typeof(OrganizationRoleEntity) };
            var queryResult =
                await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Users.GetByIdWithOptions, returnTypes.ToArray());
            if (!(queryResult[0].FirstOrDefault() is UserEntity user)) return null;
            user.Roles = queryResult[1].Select(x => x as RoleEntity).ToList();
            user.OrganizationUsers = queryResult[2].Select(x => x as OrganizationUserEntity).ToList();
            user.VirtualRoles = queryResult[3].Select(x => x as OrganizationRoleEntity).ToList();
            return user;
        }

        public async Task<UserEntity> CreateAsync(UserEntity user)
        {
            var result = (await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Insert, user)).ToList();
            user.ID = result.FirstOrDefault();
            return user;
        }

        public async Task<UserEntity> UpdateAsync(UserEntity user)
        {
            _ = (await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Update, user)).ToList();
            return user;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Delete, new { UserId = userId });
            return true;
        }

        public async Task<bool> ActivateAsync(int userId)
        {
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.Activate, new { UserId = userId });
            return true;
        }

        public async Task<bool> DeactivateAsync(int userId)
        {
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.Deactivate, new { UserId = userId });
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(UserEntity user)
        {
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.ChangePassword, new
            {
                UserId = user.ID,
                SecurityHash = user.SecurityStamp,
                user.PasswordHash
            });
            return true;
        }
    }
}

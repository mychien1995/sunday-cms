using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Models;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

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
            var searchResult = await _dbRunner.ExecuteMultipleAsync<int, UserEntity>(ProcedureNames.Users.Search, query);
            result.Total = searchResult.Item1.Single();
            result.Result = searchResult.Item2.ToList();
            return result;
        }

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
            await _dbRunner.ExecuteAsync(ProcedureNames.Users.Insert, user);
            return user.Id;
        }

        public Task UpdateAsync(UserEntity user)
            => _dbRunner.ExecuteAsync(ProcedureNames.Users.Update, user);

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

        public Task<Option<UserEntity>> FindUserByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}

using Dapper;
using Sunday.Core.DataAccess.Database;
using Sunday.Core.DataAccess.Models.Users;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.Users;
using Sunday.Core.Exceptions;
using Sunday.Core.Models;
using Sunday.Core.Models.Users;
using Sunday.Core.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.DataAccess.Repositories
{
    [ServiceTypeOf(typeof(IUserRepository))]
    public class DefaultUserRepository : IUserRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultUserRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<ApplicationUser> FindUserByNameAsync(string username)
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationUser>(ProcedureNames.Users.FindUserByUserName, new { Username = username });
            return result.FirstOrDefault();
        }

        public ApplicationUser GetUserById(int userId)
        {
            var result = _dbRunner.Execute<ApplicationUser>(ProcedureNames.Users.GetById, new { UserId = userId });
            return result.FirstOrDefault();
        }

        public ApplicationUser GetUserWithOptions(int userId, GetUserOptions option = null)
        {
            ApplicationUser user = null;
            if (option == null) option = new GetUserOptions();
            var queryResult = _dbRunner.ExecuteMultiple(ProcedureNames.Users.GetByIdWithOptions, new Type[] { typeof(ApplicationUser), typeof(ApplicationRole) }, new
            {
                UserId = userId,
                option.FetchRoles,
                option.FetchOrganizations
            });
            if (queryResult.Count > 0)
            {
                user = queryResult[0].FirstOrDefault() as ApplicationUser;
            }
            if (queryResult.Count > 1)
            {
                user.Roles = queryResult[1].Select(x => x as ApplicationRole).ToList();
            }
            return user;
        }

        public async Task<SearchResult<ApplicationUser>> QueryUsers(UserQuery query)
        {
            var result = new SearchResult<ApplicationUser>();
            var dapperQuery = query.MapTo<DapperUserQuery>();
            var param = dapperQuery.ToDapperParameters();
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Users.Search, new Type[] { typeof(int), typeof(ApplicationUser) }, param);
            result.Total = searchResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = searchResult[1].Select(x => (ApplicationUser)x);
            return result;
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
            var RoleIds = string.Join(",", user.Roles.Select(x => x.ID));
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Insert, new
            {
                user.UserName,
                user.Fullname,
                user.Email,
                user.Phone,
                user.IsActive,
                user.Domain,
                user.EmailConfirmed,
                user.CreatedBy,
                user.UpdatedBy,
                user.SecurityStamp,
                user.PasswordHash,
                RoleIds
            });
            if (!result.Any()) return null;
            user.ID = result.FirstOrDefault();
            return user;
        }

        public async Task<ApplicationUser> UpdateUser(ApplicationUser user)
        {
            var RoleIds = string.Join(",", user.Roles.Select(x => x.ID));
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Update, new
            {
                user.ID,
                user.Fullname,
                user.Email,
                user.Phone,
                user.IsActive,
                user.UpdatedBy,
                user.UpdatedDate,
                RoleIds
            });
            if (!result.Any()) return null;
            user.ID = result.FirstOrDefault();
            return user;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Delete, new { UserId = userId });
            return true;
        }

        public async Task FetchUserRoles(List<ApplicationUser> users)
        {
            if (users == null || !users.Any()) return;
            var userIds = string.Join(',', users.Select(x => x.ID));
            var dbResult = await _dbRunner.ExecuteAsync<FetchRoleResult>(ProcedureNames.Users.FetchRoles, new { UserIds = userIds });
            var groupedResult = dbResult.GroupBy(x => x.UserId).ToDictionary(x => x.Key);
            foreach (var userId in groupedResult)
            {
                var matchingUser = users.FirstOrDefault(x => x.ID == userId.Key);
                if (matchingUser == null) continue;
                matchingUser.Roles = userId.Value.Select(x => new ApplicationRole()
                {
                    Code = x.Code,
                    ID = x.RoleId,
                    RoleName = x.RoleName
                }).ToList();
            }
        }

        public async Task<bool> ActivateUser(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) throw new EntityNotFoundException("User not found");
            if (user.IsActive) throw new EntityNotFoundException("User already activated");
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.Activate, new { UserId = userId });
            return true;
        }
        public async Task<bool> DeactivateUser(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) throw new EntityNotFoundException("User not found");
            if (!user.IsActive) throw new EntityNotFoundException("User already deactivated");
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.Deactivate, new { UserId = userId });
            return true;
        }

        public async Task<bool> UpdatePassword(ApplicationUser user)
        {
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.ChangePassword, new
            {
                UserId = user.ID,
                SecurityHash = user.SecurityStamp,
                PasswordHash = user.PasswordHash
            });
            return true;
        }
    }
}

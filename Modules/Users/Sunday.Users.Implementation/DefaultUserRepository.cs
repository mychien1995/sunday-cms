using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.VirtualRoles;
using Sunday.Core.Exceptions;
using Sunday.Core.Models;
using Sunday.DataAccess.SqlServer;
using Sunday.Organizations.Core;
using Sunday.Organizations.Core.Models;
using Sunday.Users.Application;
using Sunday.Users.Core;
using Sunday.Users.Core.Models;
using Sunday.VirtualRoles.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Database;

namespace Sunday.Users.Implementation
{
    [ServiceTypeOf(typeof(IUserRepository))]
    public class DefaultUserRepository : IUserRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultUserRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public virtual async Task<ApplicationUser> FindUserByNameAsync(string username)
        {
            var queryResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Users.FindUserByUserName, new Type[] { typeof(ApplicationUser),
                typeof(ApplicationRole) }, new { Username = username });
            var user = queryResult[0].FirstOrDefault() as ApplicationUser;
            if (user != null)
                user.Roles = queryResult[1].Select(x => x as ApplicationRole).Cast<IApplicationRole>().ToList();
            return user;
        }

        public virtual async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            var queryResult = await _dbRunner.ExecuteAsync<ApplicationUser>(ProcedureNames.Users.FindUserByEmail, new { Email = email });
            var user = queryResult.FirstOrDefault();
            return user;
        }

        public virtual ApplicationUser GetUserById(int userId)
        {
            ApplicationUser user = null;
            var returnTypes = new List<Type>() { typeof(ApplicationUser), typeof(ApplicationRole), typeof(OrganizationUserEntity), typeof(OrganizationRole) };
            var queryResult = _dbRunner.ExecuteMultiple(ProcedureNames.Users.GetByIdWithOptions, returnTypes.ToArray(),
                new
                {
                    UserId = userId,
                    FetchRoles = true,
                    FetchOrganizations = true,
                    FetchVirtualRoles = true
                });
            if (queryResult.Count > 0)
            {
                user = queryResult[0].FirstOrDefault() as ApplicationUser;
            }
            if (queryResult.Count > 1)
            {
                user.Roles = queryResult[1].Select(x => x as ApplicationRole).Cast<IApplicationRole>().ToList();
            }
            if (queryResult.Count > 2)
            {
                user.OrganizationUsers = queryResult[2].Select(x => x as OrganizationUserEntity).Select(x =>
                {
                    var organizationUser = x.MapTo<ApplicationOrganizationUser>();
                    organizationUser.Organization = new ApplicationOrganization();
                    organizationUser.Organization.OrganizationName = x.OrganizationName;
                    organizationUser.Organization.ID = x.OrganizationId;
                    return organizationUser;
                }).Cast<IApplicationOrganizationUser>().ToList();
            }
            if (queryResult.Count > 3)
            {
                user.VirtualRoles = queryResult[3].Select(x => x as IOrganizationRole).ToList();
            }
            return user;
        }

        public virtual async Task<SearchResult<ApplicationUser>> QueryUsers(UserQuery query)
        {
            var result = new SearchResult<ApplicationUser>();
            var dapperQuery = query.MapTo<DapperUserQuery>();
            var param = dapperQuery.ToDapperParameters();
            var searchResult = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Users.Search, new Type[] { typeof(int), typeof(ApplicationUser) }, param);
            result.Total = searchResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = searchResult[1].Select(x => (ApplicationUser)x);
            return result;
        }

        public virtual async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
            var RoleIds = string.Join(",", user.Roles.Select(x => x.ID));
            var param = new CreateUserDynamicParameter(user);
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Insert, param);
            if (!result.Any()) return null;
            user.ID = result.FirstOrDefault();
            return user;
        }

        public virtual async Task<ApplicationUser> UpdateUser(ApplicationUser user)
        {
            var RoleIds = string.Join(",", user.Roles.Select(x => x.ID));
            var param = new UpdateUserDynamicParamter(user);
            var result = await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Update, param);
            if (!result.Any()) return null;
            user.ID = result.FirstOrDefault();
            return user;
        }

        public virtual async Task<ApplicationUser> UpdateAvatar(int userId, string blobIdentifier)
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationUser>(ProcedureNames.Users.UpdateAvatar, new
            {
                UserId = userId,
                BlobUri = blobIdentifier
            });
            if (!result.Any()) return null;
            return result.FirstOrDefault();
        }

        public virtual async Task<bool> DeleteUser(int userId)
        {
            await _dbRunner.ExecuteAsync<int>(ProcedureNames.Users.Delete, new { UserId = userId });
            return true;
        }

        public virtual async Task FetchUserRoles(List<ApplicationUser> users)
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
                }).Cast<IApplicationRole>().ToList();
            }
        }

        public virtual async Task FetchVirtualRoles(List<ApplicationUser> users)
        {
            if (users == null || !users.Any()) return;
            var userIds = string.Join(',', users.Select(x => x.ID));
            var dbResult = await _dbRunner.ExecuteAsync<FetchRoleResult>(ProcedureNames.Users.FetchVirtualRoles, new { UserIds = userIds });
            var groupedResult = dbResult.GroupBy(x => x.UserId).ToDictionary(x => x.Key);
            foreach (var userId in groupedResult)
            {
                var matchingUser = users.FirstOrDefault(x => x.ID == userId.Key);
                if (matchingUser == null) continue;
                matchingUser.VirtualRoles = userId.Value.Select(x => new OrganizationRole()
                {
                    ID = x.RoleId,
                    RoleName = x.RoleName
                }).Cast<IOrganizationRole>().ToList();
            }
        }

        public virtual async Task<bool> ActivateUser(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) throw new EntityNotFoundException("User not found");
            if (user.IsActive) throw new EntityNotFoundException("User already activated");
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.Activate, new { UserId = userId });
            return true;
        }
        public virtual async Task<bool> DeactivateUser(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) throw new EntityNotFoundException("User not found");
            if (!user.IsActive) throw new EntityNotFoundException("User already deactivated");
            await _dbRunner.ExecuteAsync<object>(ProcedureNames.Users.Deactivate, new { UserId = userId });
            return true;
        }

        public virtual async Task<bool> UpdatePassword(ApplicationUser user)
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

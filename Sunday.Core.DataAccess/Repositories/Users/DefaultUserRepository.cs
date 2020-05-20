using Dapper;
using Sunday.Core.DataAccess.Database;
using Sunday.Core.Domain.Users;
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

        public async Task<SearchResult<ApplicationUser>> QueryUsers(UserQuery query)
        {
            var result = new SearchResult<ApplicationUser>();
            var searchResult = await _dbRunner.ExecuteMultiple(ProcedureNames.Users.Search, new Type[] { typeof(int), typeof(ApplicationUser) });
            result.Total = searchResult[0].Select(x => (int)x).FirstOrDefault();
            result.Result = searchResult[1].Select(x => (ApplicationUser)x);
            return result;
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
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
                user.PasswordHash
            });
            if (!result.Any()) return null;
            user.ID = result.FirstOrDefault();
            return user;
        }
    }
}

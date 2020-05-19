using Dapper;
using Sunday.Core.DataAccess.Database;
using Sunday.Core.Domain.Users;
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
            var result = await _dbRunner.SelectAsync<ApplicationUser>(ProcedureNames.FindUserByUserName, new { Username = username });
            return result.FirstOrDefault();
        }
    }
}

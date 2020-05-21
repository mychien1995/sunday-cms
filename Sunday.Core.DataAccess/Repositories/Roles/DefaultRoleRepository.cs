using Sunday.Core.DataAccess.Database;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.DataAccess.Repositories.Roles
{
    [ServiceTypeOf(typeof(IRoleRepository))]
    public class DefaultRoleRepository : IRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultRoleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationRole>(ProcedureNames.Roles.GetAll);
            return result;
        }
    }
}

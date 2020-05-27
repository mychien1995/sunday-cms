using Sunday.Core;
using Sunday.Core.Domain.Roles;
using Sunday.DataAccess.SqlServer;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunday.Users.Application;

namespace Sunday.Users.Implementation
{
    [ServiceTypeOf(typeof(IRoleRepository))]
    public class DefaultRoleRepository : IRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultRoleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async virtual Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationRole>(ProcedureNames.Roles.GetAll);
            return result;
        }

        public async virtual Task<ApplicationRole> GetRoleById(int roleId)
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationRole>(ProcedureNames.Roles.GetById, new { RoleId = roleId });
            return result.FirstOrDefault();
        }
    }
}

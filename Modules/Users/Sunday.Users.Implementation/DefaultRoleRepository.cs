using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.Users.Application;
using Sunday.Users.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.DataAccess.SqlServer.Database;

namespace Sunday.Users.Implementation
{
    [ServiceTypeOf(typeof(IRoleRepository))]
    public class DefaultRoleRepository : IRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        private static ConcurrentDictionary<string, ApplicationRole> _roleLookup = new ConcurrentDictionary<string, ApplicationRole>();
        public DefaultRoleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async virtual Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationRole>(ProcedureNames.Roles.GetAll);
            foreach (var role in result)
            {
                _roleLookup.TryAdd(role.Code, role);
            }
            return result;
        }

        public async virtual Task<ApplicationRole> GetRoleById(int roleId)
        {
            var cachedValue = _roleLookup.FirstOrDefault(c => c.Value.ID == roleId);
            if (cachedValue.Value != null) return cachedValue.Value;
            var result = await _dbRunner.ExecuteAsync<ApplicationRole>(ProcedureNames.Roles.GetById, new { RoleId = roleId });
            return result.FirstOrDefault();
        }

        public async virtual Task<ApplicationRole> GetRoleByCode(string roleCode)
        {
            if (_roleLookup.ContainsKey(roleCode)) return _roleLookup[roleCode];
            await GetAllRolesAsync();
            return _roleLookup[roleCode];
        }
    }
}

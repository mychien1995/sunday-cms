using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.Foundation.Application.Repositories;
using Sunday.Foundation.Entities;

namespace Sunday.Foundation.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IRoleRepository))]
    internal class DefaultRoleRepository : IRoleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        private static readonly ConcurrentDictionary<string, RoleEntity> _roleLookup = new ConcurrentDictionary<string, RoleEntity>();
        public DefaultRoleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<IEnumerable<RoleEntity>> GetAllAsync()
        {
            var result = (await _dbRunner.ExecuteAsync<RoleEntity>(ProcedureNames.Roles.GetAll)).ToList();
            foreach (var role in result)
            {
                _roleLookup.TryAdd(role.Code, role);
            }
            return result;
        }

        public async Task<RoleEntity> GetRoleByIdAsync(int roleId)
        {
            var (_, value) = _roleLookup.FirstOrDefault(c => c.Value.ID == roleId);
            if (value != null) return value;
            var result = await _dbRunner.ExecuteAsync<RoleEntity>(ProcedureNames.Roles.GetById, new { RoleId = roleId });
            return result.FirstOrDefault();
        }

        public async Task<RoleEntity> GetRoleByCodeAsync(string roleCode)
        {
            if (_roleLookup.ContainsKey(roleCode)) return _roleLookup[roleCode];
            await GetAllAsync();
            return _roleLookup[roleCode];
        }
    }
}

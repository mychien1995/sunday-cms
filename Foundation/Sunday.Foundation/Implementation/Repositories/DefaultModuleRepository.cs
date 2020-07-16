using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.Foundation.Application.Repositories;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Implementation.Repositories.Entities;

namespace Sunday.Foundation.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IModuleRepository))]
    internal class DefaultModuleRepository : IModuleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultModuleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public async Task<List<ModuleEntity>> GetAllAsync()
        {
            var result = await _dbRunner.ExecuteAsync<ModuleEntity>(ProcedureNames.Modules.GetAll);
            return result.ToList();
        }
    }
}

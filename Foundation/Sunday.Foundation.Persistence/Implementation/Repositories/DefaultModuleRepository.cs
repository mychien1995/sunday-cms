using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Implementation.Repositories
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
            => (await _dbRunner.ExecuteAsync<ModuleEntity>(ProcedureNames.Modules.GetAll)).ToList();
    }
}

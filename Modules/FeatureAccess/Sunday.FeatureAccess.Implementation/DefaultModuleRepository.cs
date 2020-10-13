using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.FeatureAccess.Application;
using Sunday.FeatureAccess.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.DataAccess.SqlServer.Database;

namespace Sunday.FeatureAccess.Implementation
{
    [ServiceTypeOf(typeof(IModuleRepository))]
    public class DefaultModuleRepository : IModuleRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultModuleRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public virtual async Task<List<ApplicationModule>> GetAllModules()
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationModule>(ProcedureNames.Modules.GetAll);
            return result.ToList();
        }
    }
}

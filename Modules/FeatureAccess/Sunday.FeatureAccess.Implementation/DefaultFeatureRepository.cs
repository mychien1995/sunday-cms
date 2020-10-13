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
    [ServiceTypeOf(typeof(IFeatureRepository))]
    public class DefaultFeatureRepository : IFeatureRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultFeatureRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<List<ApplicationFeature>> GetFeaturesByModules(List<int> moduleIds)
        {
            var result = await _dbRunner.ExecuteAsync<ApplicationFeature>(ProcedureNames.Features.GetByModules, new
            {
                ModulesIds = string.Join(",", moduleIds)
            });
            return result.ToList();
        }
    }
}

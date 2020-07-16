using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.DataAccess.SqlServer;
using Sunday.Foundation.Application.Repositories;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Entities;

namespace Sunday.Foundation.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IFeatureRepository))]
    internal class DefaultFeatureRepository : IFeatureRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultFeatureRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<List<FeatureEntity>> GetFeaturesByModules(List<int> moduleIds)
        {
            var result = await _dbRunner.ExecuteAsync<FeatureEntity>(ProcedureNames.Features.GetByModules, new
            {
                ModulesIds = string.Join(",", moduleIds)
            });
            return result.ToList();
        }
    }
}

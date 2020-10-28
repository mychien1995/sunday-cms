using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.DataAccess.SqlServer.Extensions;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IFeatureRepository))]
    internal class DefaultFeatureRepository : IFeatureRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultFeatureRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }
        public async Task<List<FeatureEntity>> GetFeaturesByModules(List<Guid> moduleIds)
            => (await _dbRunner.ExecuteAsync<FeatureEntity>(ProcedureNames.Features.GetByModules, new
            {
                ModulesIds = moduleIds.ToDatabaseList()
            })).ToList();
    }
}

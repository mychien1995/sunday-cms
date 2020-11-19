using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.DataAccess.SqlServer.Database;
using Sunday.Foundation.Implementation;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;
using Sunday.Foundation.Persistence.Implementation.DapperParameters;

namespace Sunday.Foundation.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IEntityAccessRepository))]
    public class DefaultEntityAccessRepository : IEntityAccessRepository
    {
        private readonly StoredProcedureRunner _dbRunner;

        public DefaultEntityAccessRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public Task Save(Guid entityId, string entityType, EntityAccessEntity[] organizations)
        => _dbRunner.ExecuteAsync(ProcedureNames.EntityAccess.Save,
            new SaveEntityAccessParameter(entityId, entityType, organizations));

        public Task<EntityAccessEntity[]> GetEntityAccess(Guid entityId, string entityType)
            => _dbRunner.ExecuteAsync<EntityAccessEntity>(ProcedureNames.EntityAccess.GetByEntity,
                new { entityId, entityType }).MapResultTo(rs => rs.ToArray());

        public Task<EntityAccessEntity[]> GetEntityAccessByOrganization(Guid organizationId, string entityType)
            => _dbRunner.ExecuteAsync<EntityAccessEntity>(ProcedureNames.EntityAccess.GetByOrganization,
                new { organizationId, entityType }).MapResultTo(rs => rs.ToArray());
    }
}

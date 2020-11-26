using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Persistence.Application.Repositories
{
    public interface IEntityAccessRepository
    {
        Task Save(Guid entityId, string entityType, EntityAccessEntity[] organizations);

        Task<EntityAccessEntity[]> GetEntityAccess(Guid entityId, string entityType);

        Task<EntityAccessEntity[]> GetEntityAccessByOrganization(Guid organization, string entityType);

        Task<Dictionary<Guid, EntityAccessEntity[]>> GetEntitiesAccess(IEnumerable<Guid> entityId, string entityType);
    }
}

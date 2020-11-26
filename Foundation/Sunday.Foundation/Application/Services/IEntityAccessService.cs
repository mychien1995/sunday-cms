using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface IEntityAccessService
    {
        Task Save(EntityAccess entityAccess);

        Task<Option<EntityAccess>> GetByEntity(Guid entityId, string entityType);

        Task<EntityAccessFlat[]> GetByOrganization(Guid organizationId, string entityType);
        Task<Dictionary<Guid, EntityAccessFlat[]>> GetEntitiesAccess(IEnumerable<Guid> entityId, string entityType);
    }
}

using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Application.Services
{
    public interface IEntityAccessService
    {
        Task Save(EntityAccess entityAccess);

        Task<Option<EntityAccess>> GetByEntity(Guid entityId, string entityType);

        Task<EntityAccess[]> GetByOrganization(Guid organizationId, string entityType);
    }
}

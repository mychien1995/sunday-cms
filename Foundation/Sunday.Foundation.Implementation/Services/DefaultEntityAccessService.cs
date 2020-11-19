using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.DataAccess.SqlServer.Extensions;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Persistence.Application.Repositories;
using Sunday.Foundation.Persistence.Entities;

namespace Sunday.Foundation.Implementation.Services
{
    [ServiceTypeOf(typeof(IEntityAccessService))]
    public class DefaultEntityAccessService : IEntityAccessService
    {
        private readonly IEntityAccessRepository _entityAccessRepository;

        public DefaultEntityAccessService(IEntityAccessRepository entityAccessRepository)
        {
            _entityAccessRepository = entityAccessRepository;
        }

        public Task Save(EntityAccess entityAccess)
        => _entityAccessRepository.Save(entityAccess.EntityId, entityAccess.EntityType,
            entityAccess.OrganizationAccess.Select(e => new EntityAccessEntity
            {
                OrganizationId = e.OrganizationId,
                WebsiteIds = e.WebsiteIds.ToDatabaseList(),
                OrganizationRoleIds = e.OrganizationRoleIds.ToDatabaseList()
            }).ToArray());

        public Task<Option<EntityAccess>> GetByEntity(Guid entityId, string entityType)
            => _entityAccessRepository.GetEntityAccess(entityId, entityType).MapResultTo(ToModel);

        public Task<EntityAccess[]> GetByOrganization(Guid organizationId, string entityType)
        => _entityAccessRepository.GetEntityAccessByOrganization(organizationId, entityType).MapResultTo(entities => entities.Select(e => new EntityAccess
        {
            EntityId = e.EntityId,
            EntityType = e.EntityType,
            OrganizationAccess = new[]
            {
                new EntityOrganizationAccess
                {
                    OrganizationId = organizationId,
                    OrganizationRoleIds = e.OrganizationRoleIds.ToStringList().Select(Guid.Parse).ToArray(),
                    WebsiteIds = e.WebsiteIds.ToStringList().Select(Guid.Parse).ToArray()
                }
            }
        }).ToArray());

        private Option<EntityAccess> ToModel(EntityAccessEntity[] entities)
        {
            if (!entities.Any()) return Option<EntityAccess>.None;
            return new EntityAccess
            {
                EntityId = entities.First().EntityId,
                EntityType = entities.First().EntityType,
                OrganizationAccess = entities.GroupBy(e => e.OrganizationId).Select(o => new EntityOrganizationAccess
                {
                    OrganizationId = o.Key,
                    WebsiteIds = o.SelectMany(k => k.WebsiteIds.ToStringList().Select(Guid.Parse).ToArray()).ToArray(),
                    OrganizationRoleIds =
                        o.SelectMany(k => k.OrganizationRoleIds.ToStringList().Select(Guid.Parse).ToArray()).ToArray()
                }).ToArray()
            };
        }
    }
}

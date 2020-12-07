using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Services;
using Sunday.Core.Domain.Interfaces;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;

namespace Sunday.ContentManagement.Implementation.Pipelines
{
    public abstract class BaseFilterEntityAccessProcessor<T> : IAsyncPipelineProcessor where T : IEntity
    {
        protected readonly IEntityAccessService EntityAccessService;
        protected readonly ISundayContext SundayContext;
        protected readonly IWebsiteService WebsiteService;

        protected BaseFilterEntityAccessProcessor(IEntityAccessService entityAccessService, ISundayContext sundayContext, IWebsiteService websiteService)
        {
            this.EntityAccessService = entityAccessService;
            this.SundayContext = sundayContext;
            this.WebsiteService = websiteService;
        }
        public async Task<T[]> FilterEntities(T[] entities, Guid? orgId, Guid? websiteId, string entityType)
        {
            var currentUser = SundayContext.CurrentUser!;
            var organizationId = currentUser.IsOrganizationMember() ? SundayContext.CurrentOrganization?.Id :
                orgId;
            Dictionary<Guid, EntityAccessFlat[]> entityAccesses = await EntityAccessService.GetEntitiesAccess(entities.Select(t => t.Id), entityType);
            if (!entityAccesses.Any()) return entities;
            if (websiteId.HasValue)
            {
                var websiteOpt = await WebsiteService.GetByIdAsync(websiteId.Value);
                if (websiteOpt.IsNone) throw new ArgumentException($"Website {websiteId} does not exist");
                var website = websiteOpt.Get();
                if (currentUser.IsOrganizationMember() && organizationId != null && organizationId != website.OrganizationId)
                    throw new UnauthorizedAccessException($"You don't have access to {website.WebsiteName}");
            }
            var filteredEntities = new List<T>();
            entities.Iter(entity =>
            {
                if (entityAccesses.All(acc => acc.Key != entity.Id))
                    filteredEntities.Add(entity);
                else if (websiteId == null || entityAccesses.Any(acc =>
                     acc.Key == entity.Id && acc.Value.Any(v => v.WebsiteIds.Contains(websiteId.ToString()))))
                    filteredEntities.Add(entity);
                else if (organizationId == null || entityAccesses.Any(acc =>
                     acc.Key == entity.Id && acc.Value.Any(v => v.OrganizationId == organizationId)))
                    filteredEntities.Add(entity);
            });
            return filteredEntities.ToArray();
        }

        public abstract Task ProcessAsync(PipelineArg pipelineArg);
    }
}

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
        public async Task<T[]> FilterEntities(T[] entities, Guid? websiteId, string entityType)
        {
            var currentUser = SundayContext.CurrentUser!;
            var filteredEntities = new List<T>();
            Dictionary<Guid, EntityAccessFlat[]> entityAccesses = await EntityAccessService.GetEntitiesAccess(entities.Select(t => t.Id), entityType);
            filteredEntities.AddRange(entities.Where(t => entityAccesses.All(acc => acc.Key != t.Id)));
            if (websiteId.HasValue)
            {
                var websiteOpt = await WebsiteService.GetByIdAsync(websiteId.Value);
                if (websiteOpt.IsNone) throw new ArgumentException($"Website {websiteId} does not exist");
                var website = websiteOpt.Get();
                if (currentUser.IsOrganizationMember())
                {
                    var organizationId = SundayContext.CurrentOrganization!.Id;
                    if (organizationId != website.OrganizationId)
                        throw new UnauthorizedAccessException($"You don't have access to {website.WebsiteName}");
                    filteredEntities.AddRange(entities.Where(t => entityAccesses.Any(e => e.Key == t.Id
                        && e.Value.Any(acc => acc.OrganizationId == organizationId && !acc.WebsiteIds.Any() || acc.WebsiteIds.Contains(websiteId.Value.ToString())))));
                }
            }
            else if (currentUser.IsOrganizationMember())
            {
                filteredEntities.AddRange(entities.Where(t => entityAccesses.Any(e => e.Key == t.Id
                    && e.Value.Any(acc => acc.OrganizationId == SundayContext.CurrentOrganization!.Id))));
            }
            else return entities;
            return filteredEntities.ToArray();
        }

        public abstract Task ProcessAsync(PipelineArg pipelineArg);
    }
}

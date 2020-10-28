using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Foundation.Models;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(ILayoutService))]
    public class DefaultLayoutService : ILayoutService
    {
        private readonly ILayoutRepository _layoutRepository;

        public DefaultLayoutService(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        public Task<SearchResult<ApplicationLayout>> QueryAsync(LayoutQuery query)
            => _layoutRepository.QueryAsync(query).MapResultTo(rs => new SearchResult<ApplicationLayout>
            {
                Result = rs.Result.CastListTo<ApplicationLayout>(),
                Total = rs.Total
            });

        public Task<Option<ApplicationLayout>> GetByIdAsync(Guid layoutId)
            => _layoutRepository.QueryAsync(new LayoutQuery { LayoutId = layoutId }).MapResultTo(rs =>
                Optional(rs.Result.FirstOrDefault()).Map(l => l.MapTo<ApplicationLayout>()));

        public async Task CreateAsync(ApplicationLayout layout)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(layout));
            await _layoutRepository.CreateAsync(layout.MapTo<LayoutEntity>());
        }

        public async Task UpdateAsync(ApplicationLayout layout)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(layout));
            await _layoutRepository.UpdateAsync(layout.MapTo<LayoutEntity>());
        }

        public Task DeleteAsync(Guid layoutId)
            => _layoutRepository.DeleteAsync(layoutId);
    }
}

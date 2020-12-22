using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Newtonsoft.Json;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Persistence.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IRenderingService))]
    public class DefaultRenderingService : IRenderingService
    {
        private readonly IRenderingRepository _renderingRepository;

        public DefaultRenderingService(IRenderingRepository renderingRepository)
        {
            _renderingRepository = renderingRepository;
        }

        public async Task<SearchResult<Rendering>> Search(RenderingQuery query)
        {
            var renderings = await _renderingRepository.Search(query.MapTo<RenderingQueryParameter>())
                .MapResultTo(rs => new SearchResult<Rendering>(rs.Total, rs.Result.Select(ToModel).ToArray()));
            await ApplicationPipelines.RunAsync("cms.renderings.filter", new FilterRenderingsArgs(query, renderings));
            return renderings;
        }

        public async Task<Option<Rendering>> GetRenderingById(Guid id)
        {
            var entityOpt = await _renderingRepository.GetRenderingById(id);
            if (entityOpt.IsNone) return Option<Rendering>.None;
            var model = ToModel(entityOpt.Get());
            return model;
        }

        public async Task Save(Rendering rendering)
        {
            if (rendering.Id == Guid.Empty)
            {
                await ApplicationPipelines.RunAsync("cms.entity.beforeCreate", new BeforeCreateEntityArg(rendering));
            }
            else
            {
                await ApplicationPipelines.RunAsync("cms.entity.beforeUpdate", new BeforeUpdateEntityArg(rendering));
            }
            await _renderingRepository.Save(ToEntity(rendering));
        }

        public async Task Delete(Guid id)
        {
            await ApplicationPipelines.RunAsync("cms.entity.beforeDelete",
                new BeforeDeleteEntityArg(id, typeof(Rendering)));
            await _renderingRepository.Delete(id);
        }

        Rendering ToModel(RenderingEntity entity)
        {
            var model = entity.MapTo<Rendering>();
            model.Properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(entity.Properties);
            return model;
        }
        RenderingEntity ToEntity(Rendering model)
        {
            var entity = model.MapTo<RenderingEntity>();
            entity.Properties = JsonConvert.SerializeObject(model.Properties);
            return entity;
        }
    }
}

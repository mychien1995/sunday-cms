using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.CacheDependencies
{
    public class GetContentMasters : IAsyncPipelineProcessor
    {
        private readonly ICacheKeyCreator _cacheKeyCreator;
        private readonly IContentLinkService _contentLinkService;
        private readonly ITemplateService _templateService;

        public GetContentMasters(ICacheKeyCreator cacheKeyCreator,
            IContentLinkService contentLinkService, ITemplateService templateService)
        {
            _cacheKeyCreator = cacheKeyCreator;
            _contentLinkService = contentLinkService;
            _templateService = templateService;
        }

        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetEntityMastersArg)pipelineArg;
            if (arg.Entity.GetType() != typeof(Content)) return;
            var content = (arg.Entity as Content)!;
            var masters = new List<string> { _cacheKeyCreator.GetCacheKey<Template>(content.TemplateId) };
            var referencesFrom = await _contentLinkService.GetReferencesFrom(content.Id);
            masters.AddRange(referencesFrom.Select(r => _cacheKeyCreator.GetCacheKey(typeof(Content), r)));
            content.Fields.Iter(field =>
            {
                if (field.FieldValue == null) return;
                var fieldDefinition = content.Template.Fields.First(f => f.Id == field.TemplateFieldId)!;
                switch (fieldDefinition.FieldType)
                {
                    case (int)FieldTypes.RenderingArea:
                        {
                            var renderingValues = content.RenderingAreaValue(fieldDefinition.FieldName)!;
                            renderingValues.Renderings.Iter(rendering =>
                            {
                                masters.Add(_cacheKeyCreator.GetCacheKey(typeof(Rendering), rendering.RenderingId));
                            });
                            break;
                        }
                }
            });
            arg.Masters.AddRange(masters.Distinct());
        }
    }
}

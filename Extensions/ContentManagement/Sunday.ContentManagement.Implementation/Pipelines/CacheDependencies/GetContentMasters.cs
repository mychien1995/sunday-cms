using System;
using System.Collections.Generic;
using System.Linq;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.CacheDependencies
{
    public class GetContentMasters : IPipelineProcessor
    {
        private readonly ICacheKeyCreator _cacheKeyCreator;

        public GetContentMasters(ICacheKeyCreator cacheKeyCreator)
        {
            _cacheKeyCreator = cacheKeyCreator;
        }

        public void Process(PipelineArg pipelineArg)
        {
            var arg = (GetEntityMastersArg)pipelineArg;
            if (arg.Entity.GetType() != typeof(Content)) return;
            var content = (arg.Entity as Content)!;
            var masters = new List<string> { content.TemplateId.ToString(), content.ParentId.ToString() };
            content.Fields.Iter(field =>
            {
                if (field.FieldValue == null) return;
                var fieldDefinition = content.Template.Fields.First(f => f.Id == field.TemplateFieldId)!;
                switch (fieldDefinition.FieldType)
                {
                    case (int)FieldTypes.DropTree:
                        masters.Add(_cacheKeyCreator.GetCacheKey(typeof(Content), (Guid)field.FieldValue));
                        break;
                    case (int)FieldTypes.Multilist:
                        {
                            var ids = content.IdListValue(fieldDefinition.FieldName);
                            masters.AddRange(ids.Select(id => _cacheKeyCreator.GetCacheKey(typeof(Content), id)));
                            break;
                        }
                    case (int)FieldTypes.RenderingArea:
                        {
                            var renderingValues = content.RenderingAreaValue(fieldDefinition.FieldName)!;
                            renderingValues.Renderings.Iter(rendering =>
                            {
                                masters.Add(_cacheKeyCreator.GetCacheKey(typeof(Rendering), rendering.RenderingId));
                                if (rendering.Datasource != null)
                                {
                                    masters.Add(_cacheKeyCreator.GetCacheKey(typeof(Content), rendering.Datasource.Value));
                                }
                            });
                            break;
                        }
                }
            });
            arg.Masters.AddRange(masters.Distinct());
        }
    }
}

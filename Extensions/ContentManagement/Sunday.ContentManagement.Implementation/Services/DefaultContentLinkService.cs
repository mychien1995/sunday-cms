using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentLinkService))]
    public class DefaultContentLinkService : IContentLinkService
    {
        private readonly IContentLinkRepository _contentLinkRepository;

        public DefaultContentLinkService(IContentLinkRepository contentLinkRepository)
        {
            _contentLinkRepository = contentLinkRepository;
        }

        public async Task Save(Content content)
        {
            var references = new List<Guid>();
            foreach (var field in content.Fields)
            {
                if (field.FieldValue == null) return;
                var fieldDef = content.Template.Fields.First(f => f.Id == field.TemplateFieldId);
                switch (fieldDef.FieldType)
                {
                    case (int)FieldTypes.Multilist:
                        var list = content.IdListValue(fieldDef.FieldName);
                        references.AddRange(list);
                        break;
                    case (int)FieldTypes.DropTree:
                        var target = content.IdValue(fieldDef.FieldName);
                        if (target.HasValue)
                            references.Add(target!.Value);
                        break;
                    case (int)FieldTypes.RenderingArea:
                        var renderingArea = content.RenderingAreaValue(fieldDef.FieldName);
                        if (renderingArea != null)
                        {
                            var dataSources = renderingArea!.Renderings!.Where(r => r.Datasource.HasValue)
                                .Select(r => (Guid)r.Datasource!).ToList()!;
                            references.AddRange(dataSources);
                        }
                        break;
                }
            }
            await _contentLinkRepository.Save(content.Id, references.Distinct().ToArray());
        }

        public Task<Guid[]> GetReferencesTo(Guid contentId)
            => _contentLinkRepository.GetReferencesTo(contentId);

        public Task<Guid[]> GetReferencesFrom(Guid contentId)
            => _contentLinkRepository.GetReferencesFrom(contentId);
    }
}

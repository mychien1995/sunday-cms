using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.FieldTypeHandlers;
using Sunday.ContentManagement.Services;
using Sunday.Core.Configuration;

namespace Sunday.ContentManagement.Implementation.Services
{
    public class DefaultFieldTypesLoader : IFieldTypesProvider
    {
        private static readonly Dictionary<int, TemplateFieldType> Mappings = new Dictionary<int, TemplateFieldType>();

        public TemplateFieldType[] List()
            => Mappings.Select(kv => kv.Value).OrderBy(m => m.Name).ToArray();

        public TemplateFieldType Get(int code)
            => Mappings[code];

        public void Initialize(ApplicationConfiguration configuration)
        {
            var config = configuration.ConfigurationXml;
            var fieldTypesNode = config.SelectSingleNode("/configuration/templates/fieldTypes");
            if (fieldTypesNode == null || !fieldTypesNode.HasChildNodes) return;
            var childNodes = fieldTypesNode.ChildNodes;
            foreach (var node in childNodes)
            {
                var fieldTypeNode = node as XmlNode;
                if (fieldTypeNode == null) continue;
                var idStr = fieldTypeNode.Attributes["id"]?.Value;
                if(string.IsNullOrEmpty(idStr) || !int.TryParse(idStr, out var id)) continue;
                var handlerTypeStr = fieldTypeNode.InnerText;
                if(string.IsNullOrWhiteSpace(handlerTypeStr)) continue;
                var handler = Activator.CreateInstance(Type.GetType(handlerTypeStr)!) as IFieldTypeHandler;
                Mappings[id] = new TemplateFieldType(id, handler!);
            }
        }
    }
}

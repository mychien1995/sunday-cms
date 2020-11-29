using System;
using Sunday.ContentManagement.FieldTypeHandlers;

namespace Sunday.ContentManagement.Domain
{
    public class TemplateFieldType
    {
        public int Id { get; set; }
        public string Name => Handler.Name;
        public string Layout => Handler.Layout;
        public IFieldTypeHandler Handler { get; set; }

        public TemplateFieldType(int id, IFieldTypeHandler handler)
        {
            Id = id;
            Handler = handler;
        }
    }
}

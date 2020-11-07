using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(ContentField))]
    public class ContentFieldEntity
    {
        public Guid Id { get; set; }
        public string? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
        public Guid ContentId { get; set; }
    }
}

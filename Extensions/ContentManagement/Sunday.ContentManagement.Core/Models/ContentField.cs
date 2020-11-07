using System;

namespace Sunday.ContentManagement.Models
{
    public class ContentField
    {
        public Guid Id { get; set; }
        public string? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
        public Guid ContentId { get; set; }
    }
}

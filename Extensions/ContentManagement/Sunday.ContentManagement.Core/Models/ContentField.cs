using System;
using Sunday.ContentManagement.Domain;

namespace Sunday.ContentManagement.Models
{
    public class ContentField
    {
        public Guid Id { get; set; }
        public string? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
        public Guid ContentId { get; set; }
        public TemplateField? TemplateField { get; set; }
    }
}

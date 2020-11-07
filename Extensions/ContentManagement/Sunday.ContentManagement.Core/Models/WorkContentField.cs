using System;

namespace Sunday.ContentManagement.Models
{
    public class WorkContentField
    {
        public Guid Id { get; set; }
        public string? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
        public Guid WorkContentId { get; set; }
    }
}

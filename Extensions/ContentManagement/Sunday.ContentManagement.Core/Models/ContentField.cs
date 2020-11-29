using System;

namespace Sunday.ContentManagement.Models
{
    public class ContentField
    {
        public Guid Id { get; set; }
        public object? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
        public int TemplateFieldCode { get; set; }

        public ContentField()
        {
            
        }

        public ContentField(Guid id, object? fieldValue, Guid templateFieldId, int templateFieldCode)
        {
            Id = id;
            FieldValue = fieldValue;
            TemplateFieldId = templateFieldId;
            TemplateFieldCode = templateFieldCode;
        }
    }
}

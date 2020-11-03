using System;
using Sunday.ContentManagement.Domain;
using Sunday.Core;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(TemplateField))]
    public class TemplateFieldEntity
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int FieldType { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsUnversioned { get; set; }
        public string Properties { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public Guid TemplateId { get; set; }
        public bool IsRequired { get; set; }
    }
}

using System;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Templates
{
    [MappedTo(typeof(Template))]
    public class TemplateItem : BaseApiResponse
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Guid[] BaseTemplateIds { get; set; } = Array.Empty<Guid>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public TemplateFieldItem[] Fields { get; set; } = Array.Empty<TemplateFieldItem>();
    }
    
    [MappedTo(typeof(TemplateField))]
    public class TemplateFieldItem
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int FieldType { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsUnversioned { get; set; }
        public string Properties { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}

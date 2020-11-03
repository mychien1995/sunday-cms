using System;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(Template), true, nameof(BaseTemplateIds))]
    public class TemplateEntity
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string BaseTemplateIds { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        [DapperIgnoreParam]
        public TemplateFieldEntity[] Fields { get; set; } = Array.Empty<TemplateFieldEntity>();
        public bool IsUpdate { get; set; }
    }
}

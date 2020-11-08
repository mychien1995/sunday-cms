using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(Content))]
    public class ContentEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
        public int ParentType { get; set; }
        public Guid TemplateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string PublishedBy { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public WorkContentEntity[] Versions { get; set; } = Array.Empty<WorkContentEntity>();
        public ContentFieldEntity[] Fields { get; set; } = Array.Empty<ContentFieldEntity>();

        public TemplateEntity Template { get; set; } = new TemplateEntity();

    }
}

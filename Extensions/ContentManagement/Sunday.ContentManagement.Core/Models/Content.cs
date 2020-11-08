using System;
using Sunday.ContentManagement.Domain;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Models
{
    public class Content : IEntity
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

        public WorkContent[] Versions { get; set; } = Array.Empty<WorkContent>();
        public ContentField[] Fields { get; set; } = Array.Empty<ContentField>();
        public Template Template { get; set; } = new Template();
    }
}

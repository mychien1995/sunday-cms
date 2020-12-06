using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(Content))]
    public class ContentEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        [DapperIgnoreParam(DbOperation.Update)]
        public bool IsPublished { get; set; }
        public Guid ParentId { get; set; }
        public int ParentType { get; set; }
        [DapperIgnoreParam]
        public int SortOrder { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public Guid TemplateId { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public DateTime? PublishedDate { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        [DapperIgnoreParam(DbOperation.Update)]
        public string PublishedBy { get; set; } = string.Empty;

        [DapperIgnoreParam(DbOperation.Update)]
        public WorkContentEntity[] Versions { get; set; } = Array.Empty<WorkContentEntity>();
        [DapperIgnoreParam(DbOperation.Update)]
        public ContentFieldEntity[] Fields { get; set; } = Array.Empty<ContentFieldEntity>();

        [DapperIgnoreParam(DbOperation.Update)]
        public TemplateEntity Template { get; set; } = new TemplateEntity();

    }
}

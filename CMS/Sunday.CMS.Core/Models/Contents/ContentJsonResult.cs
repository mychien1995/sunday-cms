using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Contents
{
    [MappedTo(typeof(Content), true, nameof(Versions), nameof(Fields))]
    public class ContentJsonResult : BaseApiResponse
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
        public Guid ActiveVersion { get; set; }
        public ContentVersion[] Versions { get; set; } = Array.Empty<ContentVersion>();
        public ContentFieldItem[] Fields { get; set; } = Array.Empty<ContentFieldItem>();
    }

    public class ContentFieldItem
    {
        public Guid Id { get; set; }
        public string? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
    }
    public class ContentVersion
    {
        public Guid VersionId { get; set; }
        public int Version { get; set; }
    }
}

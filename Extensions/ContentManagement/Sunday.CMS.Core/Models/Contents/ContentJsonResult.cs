using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Contents
{
    [MappedTo(typeof(Content), true, nameof(Versions), nameof(Fields), nameof(Template))]
    public class ContentJsonResult : BaseApiResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string NamePath { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
        public int ParentType { get; set; }
        public Guid TemplateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string PublishedBy { get; set; } = string.Empty;
        public int? SortOrder { get; set; }
        public Guid? SelectedVersion { get; set; }
        public ContentVersion[] Versions { get; set; } = Array.Empty<ContentVersion>();
        public ContentFieldItem[] Fields { get; set; } = Array.Empty<ContentFieldItem>();
        public ContentTemplate Template { get; set; } = new ContentTemplate();
    }

    public class ContentFieldItem
    {
        public Guid Id { get; set; }
        public object? FieldValue { get; set; }
        public Guid TemplateFieldId { get; set; }
        public int TemplateFieldCode { get; set; }
    }
    public class ContentVersion
    {
        public Guid VersionId { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public static ContentVersion New(WorkContent workContent)
        => new ContentVersion(workContent.Id, workContent.Version, workContent.IsActive, workContent.Status);
        public ContentVersion()
        {

        }
        public ContentVersion(Guid versionId, int version, bool isActive, int status)
        {
            VersionId = versionId;
            Version = version;
            IsActive = isActive;
            Status = status;
        }
    }

    public class ContentTemplate
    {
        public string TemplateName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}

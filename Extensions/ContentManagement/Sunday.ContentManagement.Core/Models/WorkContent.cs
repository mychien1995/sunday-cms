using System;

namespace Sunday.ContentManagement.Models
{
    public class WorkContent
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public int Version { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
        public ContentField[] Fields { get; set; } = Array.Empty<ContentField>();
    }
}

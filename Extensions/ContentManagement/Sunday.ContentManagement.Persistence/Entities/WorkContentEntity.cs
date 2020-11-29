using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(WorkContent))]
    public class WorkContentEntity
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

        public ContentFieldEntity[] Fields { get; set; } = Array.Empty<ContentFieldEntity>();

    }
}

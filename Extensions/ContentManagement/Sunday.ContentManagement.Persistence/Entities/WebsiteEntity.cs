using System;
using Sunday.ContentManagement.Domain;
using Sunday.Core;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(ApplicationWebsite))]
    public class WebsiteEntity
    {
        public Guid Id { get; set; }
        public string WebsiteName { get; set; } = string.Empty;
        public string HostNames { get; set; } = string.Empty;
        public Guid OrganizationId { get; set; }
        public Guid LayoutId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}

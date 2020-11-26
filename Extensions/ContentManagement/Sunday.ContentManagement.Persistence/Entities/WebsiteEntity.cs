using System;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;

namespace Sunday.ContentManagement.Persistence.Entities
{
    [MappedTo(typeof(ApplicationWebsite), true, nameof(PageDesignMappings))]
    public class WebsiteEntity
    {
        public Guid Id { get; set; }
        public string WebsiteName { get; set; } = string.Empty;
        public string HostNames { get; set; } = string.Empty;
        public string PageDesignMappings { get; set; } = string.Empty;
        [DapperIgnoreParam(DbOperation.Update)]
        public Guid OrganizationId { get; set; }
        public Guid LayoutId { get; set; }
        public bool IsActive { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public DateTime CreatedDate { get; set; }
        [DapperIgnoreParam(DbOperation.Update)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        [DapperIgnoreParam]
        public bool IsDeleted { get; set; }
    }
}

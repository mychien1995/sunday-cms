using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganization : IEntity
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public List<string> HostNames { get; set; } = new List<string>();
        public string? LogoBlobUri { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<ApplicationModule> Modules { get; set; } = new List<ApplicationModule>();
    }
}

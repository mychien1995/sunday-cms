using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganization : IEntity
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        public List<string> HostNames { get; set; } = new List<string>();
        public string? LogoBlobUri { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<ApplicationModule> Modules { get; set; } = new List<ApplicationModule>();

        public ApplicationOrganization(Guid id, string organizationName, string? description, string? logoBlobUri,
            DateTime createdDate, DateTime updatedDate, string createdBy, string updatedBy, bool isActive, bool isDeleted)
        {
            Id = id;
            OrganizationName = organizationName;
            Description = description;
            LogoBlobUri = logoBlobUri;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

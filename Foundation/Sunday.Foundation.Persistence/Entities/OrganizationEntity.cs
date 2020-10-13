using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationEntity
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string? Description { get; set; }
        public string? Properties { get; set; }
        public string? LogoBlobUri { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string? HostNames { get; set; }
        public List<ModuleEntity> Modules { get; set; } = new List<ModuleEntity>();

        public OrganizationEntity(Guid id, string organizationName, string? description, string? properties,
            string? logoBlobUri, DateTime createdDate, DateTime updatedDate, string createdBy, string updatedBy, bool isActive, bool isDeleted, string? hostNames)
        {
            Id = id;
            OrganizationName = organizationName;
            Description = description;
            Properties = properties;
            LogoBlobUri = logoBlobUri;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            IsActive = isActive;
            IsDeleted = isDeleted;
            HostNames = hostNames;
        }
    }
}

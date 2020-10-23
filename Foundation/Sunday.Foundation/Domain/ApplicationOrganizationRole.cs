using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganizationRole : IEntity
    {
        public ApplicationOrganizationRole(Guid id, string roleCode, string roleName, int organizationId, ApplicationOrganization organization,
            string? description, DateTime createdDate, DateTime updatedDate, string createdBy, string updatedBy)
        {
            Id = id;
            RoleCode = roleCode;
            RoleName = roleName;
            OrganizationId = organizationId;
            Organization = organization;
            Description = description;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
        }

        public Guid Id { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public ApplicationOrganization Organization { get; set; }
        public List<ApplicationFeature> Features { get; set; } = new List<ApplicationFeature>();
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationOrganizationRole))]
    public class OrganizationRoleEntity
    {
        public Guid Id { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationEntity Organization { get; set; }
        public List<FeatureEntity> Features { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public OrganizationRoleEntity(Guid id, string roleCode, string roleName, int organizationId, 
            OrganizationEntity organization, List<FeatureEntity> features, string? description, DateTime createdDate, DateTime updatedDate, string createdBy, string updatedBy)
        {
            Id = id;
            RoleCode = roleCode;
            RoleName = roleName;
            OrganizationId = organizationId;
            Organization = organization;
            Features = features;
            Description = description;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
        }
    }
}

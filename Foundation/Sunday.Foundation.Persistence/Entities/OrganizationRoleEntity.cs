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
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public Guid OrganizationId { get; set; }
        public OrganizationEntity? Organization { get; set; }
        public List<FeatureEntity> Features { get; set; } = new List<FeatureEntity>();
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        public OrganizationRoleEntity()
        {
            
        }

        public OrganizationRoleEntity(Guid id, Guid organizationId, string name)
        {
            Id = id;
            OrganizationId = organizationId;
            RoleName = name;
        }
    }
}

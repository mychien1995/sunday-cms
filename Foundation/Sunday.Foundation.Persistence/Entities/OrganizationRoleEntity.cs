using System;
using System.Collections.Generic;
using System.Linq;
using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;
using Sunday.Foundation.Domain;
using Sunday.Foundation.Persistence.Extensions;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationOrganizationRole))]
    public class OrganizationRoleEntity
    {
        public Guid Id { get; set; }
        [DapperIgnoreParam]
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;

        [DapperIgnoreParam(DbOperation.Update)]
        public Guid OrganizationId { get; set; }
        [DapperIgnoreParam]
        public OrganizationEntity? Organization { get; set; }
        [DapperIgnoreParam]
        public List<FeatureEntity> Features { get; set; } = new List<FeatureEntity>();

        public string FeatureIds => Features.Select(f => f.Id).ToDatabaseList();
        public string? Description { get; set; }

        [DapperIgnoreParam(DbOperation.Update)]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [DapperIgnoreParam(DbOperation.Update)]
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

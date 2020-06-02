using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.VirtualRoles;
using System;
using System.Collections.Generic;

namespace Sunday.VirtualRoles.Core
{
    public class OrganizationRole : IOrganizationRole
    {
        public OrganizationRole()
        {
            Features = new List<IApplicationFeature>();
        }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public IApplicationOrganization Organization { get; set; }
        public List<IApplicationFeature> Features { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int ID { get; set; }
    }
}

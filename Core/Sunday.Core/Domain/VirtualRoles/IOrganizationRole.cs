using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Domain.Organizations;
using System.Collections.Generic;

namespace Sunday.Core.Domain.VirtualRoles
{
    public interface IOrganizationRole : IEntity
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public IApplicationOrganization Organization { get; set; }
        public List<IApplicationFeature> Features { get; set; }
        public string Description { get; set; }
    }
}

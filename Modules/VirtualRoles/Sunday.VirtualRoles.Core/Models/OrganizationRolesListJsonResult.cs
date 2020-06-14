using Sunday.Core;
using System.Collections.Generic;

namespace Sunday.VirtualRoles.Core.Models
{
    public class OrganizationRolesListJsonResult : BaseApiResponse
    {
        public OrganizationRolesListJsonResult() : base()
        {
            Roles = new List<OrganizationRoleItem>();
        }
        public int Total { get; set; }
        public IEnumerable<OrganizationRoleItem> Roles { get; set; }
    }

    [MappedTo(typeof(OrganizationRole))]
    public class OrganizationRoleItem : IOrganizationRoleJsonResult
    {
        public OrganizationRoleItem()
        {
            FeatureIds = new List<int>();
        }
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
        public List<int> FeatureIds { get; set; }
    }
}

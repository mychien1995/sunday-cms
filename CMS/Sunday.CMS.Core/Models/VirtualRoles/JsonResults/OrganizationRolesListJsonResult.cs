using Sunday.Core;
using Sunday.VirtualRoles.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.VirtualRoles
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
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
    }
}

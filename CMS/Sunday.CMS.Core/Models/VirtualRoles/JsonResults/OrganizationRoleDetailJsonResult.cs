using Sunday.Core;
using Sunday.VirtualRoles.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    [MappedTo(typeof(OrganizationRole))]
    public class OrganizationRoleDetailJsonResult : BaseApiResponse, IOrganizationRoleJsonResult
    {
        public OrganizationRoleDetailJsonResult()
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

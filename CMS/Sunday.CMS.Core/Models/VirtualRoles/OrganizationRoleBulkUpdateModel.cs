using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    public class OrganizationRoleBulkUpdateModel
    {
        public OrganizationRoleBulkUpdateModel()
        {
            Roles = new List<OrganizationRoleMutationModel>();
        }
        public List<OrganizationRoleMutationModel> Roles { get; set; }
    }
}

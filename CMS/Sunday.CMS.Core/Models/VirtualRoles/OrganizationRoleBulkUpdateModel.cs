using System.Collections.Generic;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    public class OrganizationRoleBulkUpdateModel
    {
        public List<OrganizationRoleMutationModel> Roles { get; set; } = new List<OrganizationRoleMutationModel>();
    }
}

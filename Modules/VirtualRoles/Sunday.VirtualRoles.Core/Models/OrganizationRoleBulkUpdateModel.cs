using System.Collections.Generic;

namespace Sunday.VirtualRoles.Core.Models
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

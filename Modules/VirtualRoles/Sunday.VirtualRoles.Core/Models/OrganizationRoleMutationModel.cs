using Sunday.Core;
using System.Collections.Generic;

namespace Sunday.VirtualRoles.Core.Models
{
    [MappedTo(typeof(OrganizationRole))]
    public class OrganizationRoleMutationModel
    {
        public OrganizationRoleMutationModel()
        {
            FeatureIds = new List<int>();
        }
        public int ID { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }

        public List<int> FeatureIds { get; set; }
    }
}

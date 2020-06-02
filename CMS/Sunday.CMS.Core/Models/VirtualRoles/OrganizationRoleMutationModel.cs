using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    public class OrganizationRoleMutationModel
    {
        public OrganizationRoleMutationModel()
        {
            FeatureIds = new List<int>();
        }
        public int ID { get; set; }
        public string RoleName { get; set; }

        public List<int> FeatureIds { get; set; }
    }
}

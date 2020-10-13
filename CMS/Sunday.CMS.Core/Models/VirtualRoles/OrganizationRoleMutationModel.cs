using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    [MappedTo(typeof(ApplicationOrganizationRole))]
    public class OrganizationRoleMutationModel
    {
        public OrganizationRoleMutationModel(Guid id, 
            string roleName, Guid organizationId)
        {
            Id = id;
            RoleName = roleName;
            OrganizationId = organizationId;
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public Guid OrganizationId { get; set; }

        public List<int> FeatureIds { get; set; } = new List<int>();
    }
}

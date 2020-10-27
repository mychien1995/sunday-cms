using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    [MappedTo(typeof(ApplicationOrganizationRole))]
    public class OrganizationRoleMutationModel
    {

        public Guid? Id { get; set; }
        public Guid? OrganizationId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public List<Guid> FeatureIds { get; set; } = new List<Guid>();
    }
}

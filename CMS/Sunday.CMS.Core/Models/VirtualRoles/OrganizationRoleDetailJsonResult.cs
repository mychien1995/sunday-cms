using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    [MappedTo(typeof(ApplicationOrganizationRole))]
    public class OrganizationRoleDetailJsonResult : BaseApiResponse
    {
        public OrganizationRoleDetailJsonResult(Guid id, string roleName, 
            string createdBy, string updatedBy, long createdDate, long updatedDate)
        {
            Id = id;
            RoleName = roleName;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
        public List<int> FeatureIds { get; set; } = new List<int>();
    }
}

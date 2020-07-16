using System;
using System.Collections.Generic;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganizationRole
    {
        public ApplicationOrganizationRole()
        {
            Features = new List<ApplicationFeature>();
        }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public ApplicationOrganization Organization { get; set; }
        public List<ApplicationFeature> Features { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int ID { get; set; }
    }
}

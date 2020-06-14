using System.Collections.Generic;

namespace Sunday.VirtualRoles.Core.Models
{
    public interface IOrganizationRoleJsonResult
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
        public List<int> FeatureIds { get; set; }
    }
}

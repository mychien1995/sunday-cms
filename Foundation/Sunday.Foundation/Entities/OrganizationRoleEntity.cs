using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Foundation.Entities
{
    public class OrganizationRoleEntity
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationEntity Organization { get; set; }
        public List<FeatureEntity> Features { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int ID { get; set; }
    }
}

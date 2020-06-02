using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.VirtualRoles
{
    public interface IOrganizationRoleJsonResult
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
    }
}

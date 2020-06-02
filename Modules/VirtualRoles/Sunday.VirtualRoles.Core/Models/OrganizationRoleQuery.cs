using Sunday.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.VirtualRoles.Core.Models
{
    public class OrganizationRoleQuery : PagingCriteria
    {
        public int OrganizationId { get; set; }
    }
}

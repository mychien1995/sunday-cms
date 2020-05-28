using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Organizations
{
    public interface IApplicationOrganizationUser
    {
        public IApplicationOrganization Organization { get; set; }
        public bool IsActive { get; set; }
    }
}

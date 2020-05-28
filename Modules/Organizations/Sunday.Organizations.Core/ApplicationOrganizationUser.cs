using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Organizations.Core
{
    public class ApplicationOrganizationUser : IApplicationOrganizationUser
    {
        public IApplicationOrganization Organization { get; set; }
        public bool IsActive { get; set; }
    }
}

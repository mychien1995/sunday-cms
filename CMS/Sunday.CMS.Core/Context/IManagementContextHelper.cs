using Sunday.Core.Domain.Identity;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Context
{
    public interface IManagementContextHelper
    {
        IApplicationOrganization GetCurrentOrganization();
        IApplicationUser GetCurrentUser();
    }
}

using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Application.Organizations
{
    public interface IOrganizationAccessManager
    {
        bool AllowAccess(IApplicationUser user);
        IApplicationOrganization ResolveOrganizationFromRequest();
    }
}

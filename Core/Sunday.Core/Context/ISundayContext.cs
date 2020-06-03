using Sunday.Core.Domain.Identity;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core
{
    public interface ISundayContext
    {
        IApplicationOrganization CurrentOrganization { get; }

        IApplicationUser CurrentUser { get;}
    }
}

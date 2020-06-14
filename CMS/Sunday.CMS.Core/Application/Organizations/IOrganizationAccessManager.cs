using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;

namespace Sunday.CMS.Core.Application.Organizations
{
    public interface IOrganizationAccessManager
    {
        bool AllowAccess(IApplicationUser user);
        IApplicationOrganization ResolveOrganizationFromRequest();
    }
}

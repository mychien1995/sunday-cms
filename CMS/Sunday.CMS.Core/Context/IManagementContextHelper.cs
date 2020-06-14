using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;

namespace Sunday.CMS.Core.Context
{
    public interface IManagementContextHelper
    {
        IApplicationOrganization GetCurrentOrganization();
        IApplicationUser GetCurrentUser();
    }
}

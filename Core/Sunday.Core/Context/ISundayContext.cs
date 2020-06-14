using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;

namespace Sunday.Core
{
    public interface ISundayContext
    {
        IApplicationOrganization CurrentOrganization { get; }

        IApplicationUser CurrentUser { get; }
    }
}

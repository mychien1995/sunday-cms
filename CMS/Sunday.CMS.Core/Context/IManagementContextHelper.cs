using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Context
{
    public interface IManagementContextHelper
    {
        ApplicationOrganization? GetCurrentOrganization();
        ApplicationUser? GetCurrentUser();
    }
}

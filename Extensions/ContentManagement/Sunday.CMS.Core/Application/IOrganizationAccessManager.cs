using LanguageExt;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Application
{
    public interface IOrganizationAccessManager
    {
        bool AllowAccess(ApplicationUser user);
        Option<ApplicationOrganization> ResolveOrganizationFromRequest();
    }
}

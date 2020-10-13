using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Context
{
    public interface ISundayContext
    {
        ApplicationOrganization? CurrentOrganization { get; }

        ApplicationUser? CurrentUser { get; }
    }
}

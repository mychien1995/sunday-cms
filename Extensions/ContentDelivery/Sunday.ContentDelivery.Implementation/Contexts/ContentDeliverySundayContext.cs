using Sunday.Core;
using Sunday.Foundation.Context;
using Sunday.Foundation.Domain;

namespace Sunday.ContentDelivery.Implementation.Contexts
{
    [ServiceTypeOf(typeof(ISundayContext))]
    public class ContentDeliverySundayContext : ISundayContext
    {
        public ApplicationOrganization? CurrentOrganization { get; }
        public ApplicationUser? CurrentUser { get; }
    }
}

using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class OrganizationRoleQuery : PagingCriteria
    {
        public int OrganizationId { get; set; }
        public bool FetchFeatures { get; set; }
    }
}

using Sunday.Core.Models.Base;

namespace Sunday.VirtualRoles.Core.Models
{
    public class OrganizationRoleQuery : PagingCriteria
    {
        public int OrganizationId { get; set; }
        public bool FetchFeatures { get; set; }
    }
}

using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class OrganizationQuery : PagingCriteria
    {
        public string? Text { get; set; }
        public string? SortBy { get; set; }
        public bool? IsActive { get; set; }
        public string HostName { get; set; } = string.Empty;
    }
}

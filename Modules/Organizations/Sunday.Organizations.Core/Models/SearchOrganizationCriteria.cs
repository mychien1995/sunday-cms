using Sunday.Core;

namespace Sunday.Organizations.Core.Models
{
    [MappedTo(typeof(OrganizationQuery))]
    public class SearchOrganizationCriteria
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Text { get; set; }
        public string SortBy { get; set; }
    }
}

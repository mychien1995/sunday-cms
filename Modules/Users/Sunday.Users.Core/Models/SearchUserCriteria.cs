using Sunday.Core;
using Sunday.Core.Models;
using System.Collections.Generic;

namespace Sunday.Users.Core.Models
{
    [MappedTo(typeof(UserQuery))]
    public class SearchUserCriteria : IPagingCriteria
    {
        public SearchUserCriteria()
        {
            OrganizationIds = new List<int>();
        }
        public List<int> OrganizationIds { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Text { get; set; }
        public string SortBy { get; set; }
    }
}

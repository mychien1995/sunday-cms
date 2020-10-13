using System.Collections.Generic;
using System.Linq;
using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class UserQuery : PagingCriteria
    {
        public string? Text { get; set; }
        public string? SortBy { get; set; }
        public string ExcludeIds
        {
            get
            {
                if (ExcludeIdList == null || !ExcludeIdList.Any()) return string.Empty;
                return string.Join(',', ExcludeIdList);
            }
        }
        public string IncludeIds
        {
            get
            {
                if (IncludeIdList == null || !IncludeIdList.Any()) return string.Empty;
                return string.Join(',', IncludeIdList);
            }
        }

        public string OrganizationIdList
        {
            get
            {
                if (OrganizationIds == null || !OrganizationIds.Any()) return string.Empty;
                return string.Join(',', OrganizationIds);
            }
        }

        public string? RoleIds { get; set; }

        public virtual List<int> ExcludeIdList { get; set; } = new List<int>();

        public virtual List<int> IncludeIdList { get; set; } = new List<int>();
        public virtual List<int> OrganizationIds { get; set; } = new List<int>();
    }
}

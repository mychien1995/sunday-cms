using System.Collections.Generic;
using System.Linq;
using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class UserQuery : PagingCriteria
    {
        public string? Text { get; set; }
        public string? SortBy { get; set; }
        public string ExcludeIds => !ExcludeIdList.Any() ? string.Empty : string.Join(',', ExcludeIdList);

        public string IncludeIds => !IncludeIdList.Any() ? string.Empty : string.Join(',', IncludeIdList);

        public string OrganizationIdList => !OrganizationIds.Any() ? string.Empty : string.Join(',', OrganizationIds);

        public string? RoleIds { get; set; }

        public virtual List<int> ExcludeIdList { get; set; } = new List<int>();

        public virtual List<int> IncludeIdList { get; set; } = new List<int>();
        public virtual List<int> OrganizationIds { get; set; } = new List<int>();

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IncludeRoles { get; set; } = true;
        public bool IncludeOrganizationRoles { get; set; } = true;
    }
}

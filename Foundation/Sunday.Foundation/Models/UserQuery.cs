using System;
using System.Collections.Generic;
using System.Linq;
using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class UserQuery : PagingCriteria
    {
        public string? Text { get; set; }
        public string? SortBy { get; set; }

        public List<Guid> ExcludeIdList { get; set; } = new List<Guid>();

        public List<Guid> IncludeIdList { get; set; } = new List<Guid>();
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IncludeRoles { get; set; } = true;
        public bool IncludeOrganizationRoles { get; set; } = true;
    }
}

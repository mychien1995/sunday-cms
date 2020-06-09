using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunday.Users.Core.Models
{
    public class UserQuery
    {
        public UserQuery()
        {
            ExcludeIdList = new List<int>();
            IncludeIdList = new List<int>();
            OrganizationIds = new List<int>();
        }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Text { get; set; }
        public string SortBy { get; set; }
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

        public string RoleIds { get; set; }

        public virtual List<int> ExcludeIdList { get; set; }

        public virtual List<int> IncludeIdList { get; set; }
        public virtual List<int> OrganizationIds { get; set; }
    }
}

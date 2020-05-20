using Sunday.Core;
using Sunday.Core.Models;
using Sunday.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Users
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
    }
}

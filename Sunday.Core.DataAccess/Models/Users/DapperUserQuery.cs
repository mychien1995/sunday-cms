using Sunday.Core.DataAccess.Attributes;
using Sunday.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.DataAccess.Models.Users
{
    [MappedTo(typeof(UserQuery))]
    public class DapperUserQuery : UserQuery
    {
        [DapperIgnoreParam]
        public override List<int> ExcludeIdList { get; set; }
        [DapperIgnoreParam]
        public override List<int> IncludeIdList { get; set; }
    }
}

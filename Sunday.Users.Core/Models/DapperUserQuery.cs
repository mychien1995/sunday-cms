using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Users.Core.Models
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

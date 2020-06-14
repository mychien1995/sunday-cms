using Sunday.Core;
using Sunday.DataAccess.SqlServer.Attributes;
using System.Collections.Generic;

namespace Sunday.Users.Core.Models
{
    [MappedTo(typeof(UserQuery))]
    public class DapperUserQuery : UserQuery
    {
        [DapperIgnoreParam]
        public override List<int> ExcludeIdList { get; set; }
        [DapperIgnoreParam]
        public override List<int> IncludeIdList { get; set; }
        [DapperIgnoreParam]
        public override List<int> OrganizationIds { get; set; }
    }
}

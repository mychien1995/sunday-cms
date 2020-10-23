using Sunday.Core;
using Sunday.Users.Core;
using System.Collections.Generic;
using Sunday.Core.Pipelines;

namespace Sunday.Users.Implementation.Pipelines.Arguments
{
    public class GetAvailableRolesArg : PipelineArg
    {
        public GetAvailableRolesArg()
        {
            Roles = new List<ApplicationRole>();
        }
        public List<ApplicationRole> Roles { get; set; }
    }
}

using Sunday.Core;
using Sunday.Core.Domain.Roles;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Pipelines.Arguments
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

using System.Collections.Generic;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Pipelines.Arguments
{
    public class GetAvailableRolesArg : PipelineArg
    {
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
    }
}

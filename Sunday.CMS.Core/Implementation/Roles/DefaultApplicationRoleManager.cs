using Sunday.CMS.Core.Application.Roles;
using Sunday.CMS.Core.Models.Roles.JsonResults;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.Roles
{
    [ServiceTypeOf(typeof(IApplicationRoleManager))]
    public class DefaultApplicationRoleManager : IApplicationRoleManager
    {
        public async Task<RoleListJsonResult> GetAvailableRoles()
        {
            var arg = new GetAvailableRolesArg();
            await ApplicationPipelines.RunAsync("cms.roles.getAvailable", arg);
            var roles = arg.Roles;
            var result = new RoleListJsonResult();
            result.List = arg.Roles.Select(x => x.MapTo<RoleItem>()).ToList();
            return result;
        }
    }
}

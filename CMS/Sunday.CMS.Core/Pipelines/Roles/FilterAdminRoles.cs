using System.Linq;
using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Constants;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Pipelines.Roles
{
    public class FilterAdminRoles : IPipelineProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FilterAdminRoles(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (GetAvailableRolesArg)pipelineArg;
            var roles = arg.Roles;
            if (roles == null || !roles.Any()) return;
            var user = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            if (user!.IsInRole(RoleCodes.SystemAdmin))
            {
                arg.Roles = arg.Roles.Where(x => x.Code != RoleCodes.SystemAdmin && x.Code != RoleCodes.Developer).ToList();
            }
        }
    }
}

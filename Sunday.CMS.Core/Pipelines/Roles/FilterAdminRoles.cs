using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Identity;
using Sunday.Core.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Roles
{
    public class FilterAdminRoles
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FilterAdminRoles(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task ProcessAsync(GetAvailableRolesArg arg)
        {
            var roles = arg.Roles;
            if (roles == null || !roles.Any()) return;
            var user = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            if (user.IsInRole(RoleCodes.SystemAdmin))
            {
                arg.Roles = arg.Roles.Where(x => x.Code != RoleCodes.SystemAdmin && x.Code != RoleCodes.Developer).ToList();
            }
        }
    }
}

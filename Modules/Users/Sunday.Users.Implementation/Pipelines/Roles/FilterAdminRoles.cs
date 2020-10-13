using Microsoft.AspNetCore.Http;
using Sunday.Core;
using Sunday.Identity.Core;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Constants;

namespace Sunday.Users.Implementation.Pipelines.Roles
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

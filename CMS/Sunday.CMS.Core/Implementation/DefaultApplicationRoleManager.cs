using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Roles;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Core.Constants;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationRoleManager))]
    public class DefaultApplicationRoleManager : IApplicationRoleManager
    {
        private readonly IRoleService _roleService;

        public DefaultApplicationRoleManager(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<RoleListJsonResult> GetAvailableRoles()
        {
            var arg = new GetAvailableRolesArg();
            await ApplicationPipelines.RunAsync("cms.roles.getAvailable", arg);
            var result = new RoleListJsonResult
            {
                List = arg.Roles.Select(role =>
                {
                    var roleItem = role.MapTo<RoleItem>();
                    if (roleItem.Code == RoleCodes.OrganizationAdmin || roleItem.Code == RoleCodes.OrganizationUser)
                        roleItem.RequireOrganization = true;
                    return roleItem;
                }).ToList()
            };
            return result;
        }

        public async Task<RoleDetailJsonResult> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role.IsNone) return BaseApiResponse.ErrorResult<RoleDetailJsonResult>("Role not found");
            var result = role.Get().MapTo<RoleDetailJsonResult>();
            if (role.Get().Code == RoleCodes.OrganizationAdmin || role.Get().Code == RoleCodes.OrganizationUser)
                result.RequireOrganization = true;
            return result;
        }
    }
}

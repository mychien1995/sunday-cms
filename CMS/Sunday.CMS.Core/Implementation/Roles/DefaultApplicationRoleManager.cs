using Sunday.CMS.Core.Application.Roles;
using Sunday.CMS.Core.Models.Roles;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using Sunday.Users.Application;
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
        private readonly IRoleRepository _roleRepository;
        public DefaultApplicationRoleManager(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleListJsonResult> GetAvailableRoles()
        {
            var arg = new GetAvailableRolesArg();
            await ApplicationPipelines.RunAsync("cms.roles.getAvailable", arg);
            var roles = arg.Roles;
            var result = new RoleListJsonResult();
            result.List = arg.Roles.Select(x =>
            {
                var roleItem = x.MapTo<RoleItem>();
                if (roleItem.Code == RoleCodes.OrganizationAdmin || roleItem.Code == RoleCodes.OrganizationUser)
                    roleItem.RequireOrganization = true;
                return roleItem;
            }).ToList();
            return result;
        }

        public async Task<RoleDetailJsonResult> GetRoleById(int id)
        {
            var role = await _roleRepository.GetRoleById(id);
            var result = role.MapTo<RoleDetailJsonResult>();
            if (role.Code == RoleCodes.OrganizationAdmin || role.Code == RoleCodes.OrganizationUser)
                result.RequireOrganization = true;
            return result;
        }
    }
}

using Sunday.Core;
using Sunday.Users.Application;
using Sunday.Users.Core.Models;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation
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

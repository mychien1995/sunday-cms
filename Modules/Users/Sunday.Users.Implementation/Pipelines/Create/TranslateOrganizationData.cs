using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.VirtualRoles;
using Sunday.Organizations.Core;
using Sunday.Users.Application;
using Sunday.Users.Core;
using Sunday.Users.Implementation.Pipelines.Arguments;
using Sunday.VirtualRoles.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.Core.Constants;
using Sunday.Core.Context;

namespace Sunday.Users.Implementation.Pipelines.Create
{
    public class TranslateOrganizationData
    {
        private readonly ISundayContext _sundayContext;
        private readonly IRoleRepository _roleRepository;

        public TranslateOrganizationData(ISundayContext sundayContext, IRoleRepository roleRepository)
        {
            _sundayContext = sundayContext;
            _roleRepository = roleRepository;
        }
        public async Task ProcessAsync(BeforeCreateUserArg arg)
        {
            arg.User.Roles = arg.Input.RoleIds.Select(x => (IApplicationRole)new ApplicationRole()
            {
                ID = x
            }).ToList();
            var currentUser = _sundayContext.CurrentUser;
            if (currentUser.IsInRole(SystemRoleCodes.OrganizationAdmin) || currentUser.IsInRole(SystemRoleCodes.OrganizationUser))
            {
                var currentOrgId = _sundayContext.CurrentOrganization.ID;
                arg.User.OrganizationUsers = new List<IApplicationOrganizationUser>()
                {
                    new ApplicationOrganizationUser()
                    {
                        Organization = new ApplicationOrganization()
                        {
                            ID = currentOrgId
                        },
                        OrganizationId = currentOrgId,
                        IsActive = arg.Input.IsActive
                    }
                };
                arg.User.VirtualRoles = arg.Input.OrganizationRoleIds.Select(x => new OrganizationRole()
                {
                    ID = x,
                    OrganizationId = currentOrgId
                } as IOrganizationRole).ToList();
                arg.User.Domain = currentUser.Domain;
                arg.User.Roles = new List<IApplicationRole> { (await _roleRepository.GetRoleByCode(SystemRoleCodes.OrganizationUser)) };
            }
            else if (currentUser.IsInRole(SystemRoleCodes.SystemAdmin) || currentUser.IsInRole(SystemRoleCodes.Developer))
            {
                arg.User.Roles = arg.Input.RoleIds.Select(x => (IApplicationRole)new ApplicationRole()
                {
                    ID = x
                }).ToList();
                arg.User.OrganizationUsers = arg.Input.Organizations.Select(x => (IApplicationOrganizationUser)new ApplicationOrganizationUser()
                {
                    Organization = new ApplicationOrganization()
                    {
                        ID = x.OrganizationId
                    },
                    OrganizationId = x.OrganizationId,
                    IsActive = x.IsActive
                }).ToList();
            }
        }
    }
}

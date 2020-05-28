using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Roles;
using Sunday.Organizations.Core;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users
{
    public class TranslateMutationData
    {
        public async Task ProcessAsync(BeforeCreateUserArg arg)
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
                IsActive = x.IsActive
            }).ToList();
        }
    }
}

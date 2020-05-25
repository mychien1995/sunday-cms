using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Users;
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
            arg.User.Roles = arg.Input.RoleIds.Select(x => new ApplicationRole()
            {
                ID = x
            }).ToList();
        }
    }
}

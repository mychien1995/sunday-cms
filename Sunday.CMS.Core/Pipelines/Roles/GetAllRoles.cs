using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Roles
{
    public class GetAllRoles
    {
        private readonly IRoleRepository _roleRepository;
        public GetAllRoles(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task ProcessAsync(GetAvailableRolesArg arg)
        {
            var allRoles = await _roleRepository.GetAllRolesAsync();
            arg.Roles = allRoles.ToList();
        }
    }
}

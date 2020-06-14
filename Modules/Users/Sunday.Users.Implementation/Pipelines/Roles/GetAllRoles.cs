using Sunday.Users.Application;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.Roles
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

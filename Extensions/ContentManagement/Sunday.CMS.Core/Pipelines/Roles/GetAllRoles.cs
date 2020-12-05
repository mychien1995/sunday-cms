using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;

namespace Sunday.CMS.Core.Pipelines.Roles
{
    public class GetAllRoles : IAsyncPipelineProcessor
    {
        private readonly IRoleService _roleService;
        public GetAllRoles(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetAvailableRolesArg)pipelineArg;
            var allRoles = await _roleService.GetAllAsync();
            arg.Roles = allRoles.ToList();
        }
    }
}

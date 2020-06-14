using Microsoft.AspNetCore.Mvc;
using Sunday.Users.Application;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IApplicationRoleManager _rolesManager;
        public RolesController(IApplicationRoleManager roleManager)
        {
            _rolesManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _rolesManager.GetAvailableRoles();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _rolesManager.GetRoleById(id);
            return Ok(result);
        }
    }
}

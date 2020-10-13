using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IApplicationRoleManager _rolesManager;
        public RolesController(IApplicationRoleManager roleManager, ISundayContext context) : base(context)
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
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _rolesManager.GetRoleById(id);
            return Ok(result);
        }
    }
}

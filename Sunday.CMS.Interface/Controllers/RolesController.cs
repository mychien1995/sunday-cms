using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

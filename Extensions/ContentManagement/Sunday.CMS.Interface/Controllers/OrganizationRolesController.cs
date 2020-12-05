using System;
using Microsoft.AspNetCore.Mvc;
using Sunday.Core;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.Foundation.Context;
using OrganizationRoleQuery = Sunday.Foundation.Models.OrganizationRoleQuery;

namespace Sunday.CMS.Interface.Controllers
{
    public class OrganizationRolesController : BaseController
    {
        private readonly IOrganizationRolesManager _organizationRoleManager;
        public OrganizationRolesController(IOrganizationRolesManager organizationRoleManager, ISundayContext context) : base(context)
        {
            this._organizationRoleManager = organizationRoleManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] OrganizationRoleQuery query)
        {
            var result = await _organizationRoleManager.GetRolesList(query);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrganizationRoleMutationModel data)
        {
            var result = await _organizationRoleManager.CreateRole(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] OrganizationRoleMutationModel data)
        {
            var result = await _organizationRoleManager.UpdateRole(data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _organizationRoleManager.GetRoleById(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _organizationRoleManager.DeleteRole(id);
            return Ok(result);
        }

        [HttpPost("bulkUpdate")]
        public async Task<IActionResult> BulkUpdate([FromBody] OrganizationRoleBulkUpdateModel roles)
        {
            var result = await _organizationRoleManager.BulkUpdate(roles);
            return Ok(result);
        }
    }
}

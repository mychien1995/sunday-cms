using Sunday.CMS.Core.Application.VirtualRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.Core.Models.Base;
using Sunday.VirtualRoles.Core.Models;
using Sunday.Core;

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
        public async Task<IActionResult> Search([FromBody]OrganizationRoleQuery query)
        {
            query.OrganizationId = CurrentOrganizationId;
            var result = await _organizationRoleManager.GetRolesList(query);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]OrganizationRoleMutationModel data)
        {
            data.OrganizationId = CurrentOrganizationId;
            var result = await _organizationRoleManager.CreateRole(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]OrganizationRoleMutationModel data)
        {
            data.OrganizationId = CurrentOrganizationId;
            var result = await _organizationRoleManager.UpdateRole(data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var result = await _organizationRoleManager.GetRoleById(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var result = await _organizationRoleManager.DeleteRole(id);
            return Ok(result);
        }
    }
}

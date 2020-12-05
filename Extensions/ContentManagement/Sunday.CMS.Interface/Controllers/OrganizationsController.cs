using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Organizations;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Interface.Controllers
{
    public class OrganizationsController : BaseController
    {
        private readonly IApplicationOrganizationManager _organizationManager;
        public OrganizationsController(IApplicationOrganizationManager organizationManager, ISundayContext context) : base(context)
        {
            this._organizationManager = organizationManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] OrganizationQuery criteria)
        {
            var result = await _organizationManager.SearchOrganizations(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrganizationMutationModel data)
        {
            var result = await _organizationManager.CreateOrganization(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] OrganizationMutationModel data)
        {
            var result = await _organizationManager.UpdateOrganization(data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _organizationManager.GetOrganizationById(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _organizationManager.DeleteOrganization(id);
            return Ok(result);
        }

        [HttpPut("activate")]
        public async Task<IActionResult> Activate([FromQuery] Guid id)
        {
            var result = await _organizationManager.ActivateOrganization(id);
            return Ok(result);
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> Deactivate([FromQuery] Guid id)
        {
            var result = await _organizationManager.DeactivateOrganization(id);
            return Ok(result);
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup()
        {
            var result = await _organizationManager.GetOrganizationLookup();
            return Ok(result);
        }
    }
}

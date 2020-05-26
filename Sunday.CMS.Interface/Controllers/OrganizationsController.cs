using Sunday.CMS.Core.Application.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Models.Organizations;

namespace Sunday.CMS.Interface.Controllers
{
    public class OrganizationsController : BaseController
    {
        private readonly IApplicationOrganizationManager _organizationManager;
        public OrganizationsController(IApplicationOrganizationManager organizationManager)
        {
            this._organizationManager = organizationManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]SearchOrganizationCriteria criteria)
        {
            var result = await _organizationManager.SearchOrganizations(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]OrganizationMutationModel data)
        {
            var result = await _organizationManager.CreateOrganization(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]OrganizationMutationModel data)
        {
            var result = await _organizationManager.UpdateOrganization(data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var result = await _organizationManager.GetOrganizationById(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var result = await _organizationManager.DeactivateOrganization(id);
            return Ok(result);
        }

        [HttpPut("activate")]
        public async Task<IActionResult> Activate([FromQuery] int id)
        {
            var result = await _organizationManager.ActivateOrganization(id);
            return Ok(result);
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> Deactivate([FromQuery] int id)
        {
            var result = await _organizationManager.DeactivateOrganization(id);
            return Ok(result);
        }
    }
}

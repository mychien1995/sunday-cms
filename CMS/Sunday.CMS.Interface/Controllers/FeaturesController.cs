using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.FeatureAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class FeaturesController : BaseController
    {
        private readonly IApplicationFeatureManager _featureManager;
        public FeaturesController(IApplicationFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet("getByOrganization")]
        public async Task<IActionResult> GetByOrganization()
        {
            int organizationId = 0;
            var result = await _featureManager.GetOrganizationFeatures(organizationId);
            return Ok(result);
        }
    }
}

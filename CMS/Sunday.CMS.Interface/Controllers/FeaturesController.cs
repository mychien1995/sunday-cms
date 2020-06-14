using Microsoft.AspNetCore.Mvc;
using Sunday.Core;
using Sunday.FeatureAccess.Application;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class FeaturesController : BaseController
    {
        private readonly IApplicationFeatureManager _featureManager;
        public FeaturesController(IApplicationFeatureManager featureManager, ISundayContext context) : base(context)
        {
            _featureManager = featureManager;
        }

        [HttpGet("getByOrganization")]
        public async Task<IActionResult> GetByOrganization()
        {
            int organizationId = CurrentOrganizationId;
            var result = await _featureManager.GetOrganizationFeatures(organizationId);
            return Ok(result);
        }
    }
}

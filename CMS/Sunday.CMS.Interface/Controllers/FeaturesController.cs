using Microsoft.AspNetCore.Mvc;
using Sunday.Core;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.Foundation.Context;

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
            var result = await _featureManager.GetOrganizationFeatures(CurrentOrganizationId);
            return Ok(result);
        }
    }
}

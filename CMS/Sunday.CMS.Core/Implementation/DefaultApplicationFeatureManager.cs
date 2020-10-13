using System;
using System.Linq;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.FeatureAccess;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;

namespace Sunday.CMS.Core.Implementation
{
    [ServiceTypeOf(typeof(IApplicationFeatureManager))]
    public class DefaultApplicationFeatureManager : IApplicationFeatureManager
    {
        private readonly IFeatureService _featureService;
        private readonly IOrganizationService _organizationService;

        public DefaultApplicationFeatureManager(IFeatureService featureService, IOrganizationService organizationService)
        {
            _featureService = featureService;
            _organizationService = organizationService;
        }

        public async Task<FeatureListJsonResult> GetOrganizationFeatures(Guid organizationId)
        {
            var result = new FeatureListJsonResult();
            var organization = await _organizationService.GetOrganizationByIdAsync(organizationId);
            if (organization.IsNone)
            {
                result.AddError($"Organization {organizationId} not found");
                return result;
            }
            var searchResult = await _featureService.GetFeaturesByModules(organization.Get().Modules.Select(m => m.Id).ToList());
            result.Features = searchResult.CastListTo<FeatureItem>().ToList();
            result.Total = result.Features.Count;
            return result;
        }
    }
}

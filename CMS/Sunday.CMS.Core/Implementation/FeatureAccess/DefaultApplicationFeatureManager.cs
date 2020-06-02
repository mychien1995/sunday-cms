using Sunday.CMS.Core.Application.FeatureAccess;
using Sunday.CMS.Core.Models.FeatureAccess;
using Sunday.Core;
using Sunday.FeatureAccess.Application;
using Sunday.Organizations.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.FeatureAccess
{
    [ServiceTypeOf(typeof(IApplicationFeatureManager))]
    public class DefaultApplicationFeatureManager : IApplicationFeatureManager
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IOrganizationRepository _organizationRepository;
        public DefaultApplicationFeatureManager(IFeatureRepository featureRepository, IOrganizationRepository organizationRepository)
        {
            _featureRepository = featureRepository;
            _organizationRepository = organizationRepository;
        }
        public async Task<FeatureListJsonResult> GetOrganizationFeatures(int organizationId)
        {
            var result = new FeatureListJsonResult();
            var organization = _organizationRepository.GetById(organizationId);
            var searchResult = await _featureRepository.GetFeaturesByModules(organization.Modules.Select(x => x.ID).ToList());
            result.Features = searchResult.Select(x => x.MapTo<FeatureItem>()).ToList();
            result.Total = result.Features.Count();
            return result;
        }
    }
}

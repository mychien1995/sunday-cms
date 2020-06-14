using Sunday.Core;
using Sunday.FeatureAccess.Application;
using Sunday.FeatureAccess.Core.Models;
using Sunday.Organizations.Application;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.FeatureAccess.Implementation
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

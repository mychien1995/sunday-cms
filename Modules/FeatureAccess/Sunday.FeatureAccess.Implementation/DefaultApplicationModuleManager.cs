using Sunday.Core;
using Sunday.FeatureAccess.Application;
using Sunday.FeatureAccess.Core.Models;
using Sunday.Organizations.Application;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.FeatureAccess.Implementation
{
    [ServiceTypeOf(typeof(IApplicationModuleManager))]
    public class DefaultApplicationModuleManager : IApplicationModuleManager
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IFeatureRepository _featureRepository;

        public DefaultApplicationModuleManager(IModuleRepository moduleRepository, IOrganizationRepository organizationRepository,
            IFeatureRepository featureRepository)
        {
            _moduleRepository = moduleRepository;
            _organizationRepository = organizationRepository;
            _featureRepository = featureRepository;
        }
        public async Task<ModuleListJsonResult> GetModules()
        {
            var module = await _moduleRepository.GetAllModules();
            var result = new ModuleListJsonResult();
            result.Modules = module.Select(x => x.MapTo<ModuleItem>()).ToList();
            return result;
        }

        public virtual async Task<ModuleListJsonResult> GetOrganizationModules(int organizationId)
        {
            var result = new ModuleListJsonResult();
            var organization = _organizationRepository.GetById(organizationId);
            result.Modules = organization.Modules.Select(x => new ModuleItem()
            {
                ID = x.ID,
                ModuleCode = x.ModuleCode,
                ModuleName = x.ModuleName
            });
            var features = await _featureRepository.GetFeaturesByModules(organization.Modules.Select(x => x.ID).ToList());
            var modules = result.Modules.ToList();
            foreach (var module in modules)
            {
                module.Features = features.Where(x => x.ModuleId == module.ID).Select(x => new FeatureItem()
                {
                    ID = x.ID,
                    FeatureCode = x.FeatureCode,
                    FeatureName = x.FeatureName
                }).ToList();
            }
            result.Modules = modules;
            return result;
        }
    }
}

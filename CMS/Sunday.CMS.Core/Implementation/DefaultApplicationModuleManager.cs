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
    [ServiceTypeOf(typeof(IApplicationModuleManager))]
    public class DefaultApplicationModuleManager : IApplicationModuleManager
    {
        private readonly IModuleService _moduleService;
        private readonly IFeatureService _featureService;
        private readonly IOrganizationService _organizationService;

        public DefaultApplicationModuleManager(IModuleService moduleService, IOrganizationService organizationService, IFeatureService featureService)
        {
            _moduleService = moduleService;
            _organizationService = organizationService;
            _featureService = featureService;
        }

        public Task<ModuleListJsonResult> GetModules()
            => _moduleService.GetAllAsync().MapResultTo(list => new ModuleListJsonResult()
            {
                Modules = list.CastListTo<ModuleItem>().ToList(),
                Total = list.Count
            });

        public async Task<ModuleListJsonResult> GetOrganizationModules(Guid organizationId)
        {
            var result = new ModuleListJsonResult();
            var organizationOpt = await _organizationService.GetOrganizationByIdAsync(organizationId);
            if(organizationOpt.IsNone) throw new ArgumentException($"Organization {organizationId} not found");
            var organization = organizationOpt.Get();
            result.Modules = organization.Modules.Select(m => new ModuleItem(m.Id, m.ModuleName, m.ModuleCode)).ToList();
            var features = await _featureService.GetFeaturesByModules(organization.Modules.Select(m => m.Id).ToList());
            var modules = result.Modules.ToList();
            foreach (var module in modules)
            {
                module.Features = features.Where(f => f.ModuleId == module.Id)
                    .Select(f => new FeatureItem(f.Id, f.FeatureCode, f.FeatureName)).ToList();
            }
            result.Modules = modules;
            return result;
        }
    }
}

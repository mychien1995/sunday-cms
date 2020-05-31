using Sunday.CMS.Core.Application.FeatureAccess;
using Sunday.CMS.Core.Models.FeatureAccess;
using Sunday.Core;
using Sunday.FeatureAccess.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Implementation.FeatureAccess
{
    [ServiceTypeOf(typeof(IApplicationModuleManager))]
    public class DefaultApplicationModuleManager : IApplicationModuleManager
    {
        private readonly IModuleRepository _moduleRepository;
        public DefaultApplicationModuleManager(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        public async Task<ModuleListJsonResult> GetModules()
        {
            var module = await _moduleRepository.GetAllModules();
            var result = new ModuleListJsonResult();
            result.Modules = module.Select(x => x.MapTo<ModuleItem>()).ToList();
            return result;
        }
    }
}

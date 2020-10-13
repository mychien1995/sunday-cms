using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.FeatureAccess;

namespace Sunday.CMS.Core.Implementation
{
    public class DefaultApplicationModuleManager : IApplicationModuleManager
    {
        public Task<ModuleListJsonResult> GetModules()
        {
            throw new NotImplementedException();
        }

        public Task<ModuleListJsonResult> GetOrganizationModules(Guid organizationId)
        {
            throw new NotImplementedException();
        }
    }
}

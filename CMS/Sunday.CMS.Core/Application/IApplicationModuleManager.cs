using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.FeatureAccess;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationModuleManager
    {
        Task<ModuleListJsonResult> GetModules();

        Task<ModuleListJsonResult> GetOrganizationModules(Guid organizationId);
    }
}

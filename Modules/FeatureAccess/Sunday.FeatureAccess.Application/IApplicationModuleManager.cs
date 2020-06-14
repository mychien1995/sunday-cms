using Sunday.FeatureAccess.Core.Models;
using System.Threading.Tasks;

namespace Sunday.FeatureAccess.Application
{
    public interface IApplicationModuleManager
    {
        Task<ModuleListJsonResult> GetModules();

        Task<ModuleListJsonResult> GetOrganizationModules(int organizationId);
    }
}

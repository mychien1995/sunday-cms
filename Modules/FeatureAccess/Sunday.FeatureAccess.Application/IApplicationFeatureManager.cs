using Sunday.FeatureAccess.Core.Models;
using System.Threading.Tasks;

namespace Sunday.FeatureAccess.Application
{
    public interface IApplicationFeatureManager
    {
        Task<FeatureListJsonResult> GetOrganizationFeatures(int organizationId);
    }
}

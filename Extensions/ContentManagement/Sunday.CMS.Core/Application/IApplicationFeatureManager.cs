using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.FeatureAccess;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationFeatureManager
    {
        Task<FeatureListJsonResult> GetOrganizationFeatures(Guid organizationId);
    }
}

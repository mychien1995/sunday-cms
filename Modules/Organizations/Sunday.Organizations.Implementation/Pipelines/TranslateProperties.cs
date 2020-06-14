using Sunday.Core;
using Sunday.Core.Domain.FeatureAccess;
using Sunday.FeatureAccess.Core;
using Sunday.Organizations.Core;
using Sunday.Organizations.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.Organizations.Implementation.Pipelines
{
    public class TranslateProperties
    {
        public async Task ProcessAsync(PipelineArg arg)
        {
            var mutationData = arg["Source"] as OrganizationMutationModel;
            var organization = arg["Target"] as ApplicationOrganization;
            if (mutationData == null || organization == null) return;
            if (!string.IsNullOrEmpty(mutationData.ColorScheme))
            {
                organization.Properties.Add("color", mutationData.ColorScheme);
            }
            if (mutationData.ModuleIds == null || !mutationData.ModuleIds.Any()) organization.Modules = new List<IApplicationModule>();
            else
            {
                organization.Modules = mutationData.ModuleIds.Select(x => new ApplicationModule()
                {
                    ID = x
                } as IApplicationModule).ToList();
            }
        }
    }
}

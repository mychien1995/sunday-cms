using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Domain.Organizations;
using Sunday.FeatureAccess.Core;
using Sunday.Organizations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Organizations
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

using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
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
        }
    }
}

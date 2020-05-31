using Sunday.CMS.Core.Models.Organizations;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Core.Media.Application;
using Sunday.Organizations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Organizations
{
    public class TranslatePropertiesToModel
    {
        private readonly IBlobLinkManager _blobLinkManager;
        public TranslatePropertiesToModel(IBlobLinkManager linkManager)
        {
            _blobLinkManager = linkManager;
        }
        public async Task ProcessAsync(PipelineArg arg)
        {
            var organization = arg["Source"] as ApplicationOrganization;
            var model = arg["Target"] as IOrganizationProperties;
            if (organization == null || model == null) return;
            model.LogoLink = _blobLinkManager.GetPreviewLink(organization.LogoBlobUri);
            model.ColorScheme = organization.Properties["color"]?.ToString();
            if (arg["Target"] is OrganizationDetailJsonResult)
            {
                var detailResult = arg["Target"] as OrganizationDetailJsonResult;
                detailResult.ModuleIds = organization.Modules.Select(x => x.ID).ToList();
            }
        }
    }
}

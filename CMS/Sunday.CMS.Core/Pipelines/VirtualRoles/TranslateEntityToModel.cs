using Sunday.CMS.Core.Models.VirtualRoles;
using Sunday.Core.Domain.FeatureAccess;
using Sunday.Core.Pipelines.Arguments;
using Sunday.FeatureAccess.Core;
using Sunday.VirtualRoles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.VirtualRoles
{
    public class TranslateEntityToModel
    {
        public async Task ProcessAsync(EntityModelExchangeArg arg)
        {
            var model = arg.Model as IOrganizationRoleJsonResult;
            var entity = arg.Entity as OrganizationRole;
            if (model == null || entity == null) return;
            model.FeatureIds = entity.Features.Select(x => x.ID).ToList();
        }
    }
}

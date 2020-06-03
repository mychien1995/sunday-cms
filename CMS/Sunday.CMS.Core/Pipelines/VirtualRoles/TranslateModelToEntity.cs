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
    public class TranslateModelToEntity
    {
        public async Task ProcessAsync(BeforeUpdateEntityArg arg)
        {
            var model = arg.DataModel as OrganizationRoleMutationModel;
            var entity = arg.Entity as OrganizationRole;
            if (model == null || entity == null) return;
            entity.Features = model.FeatureIds.Select(x => new ApplicationFeature()
            {
                ID = x
            } as IApplicationFeature).ToList();
        }
    }
}

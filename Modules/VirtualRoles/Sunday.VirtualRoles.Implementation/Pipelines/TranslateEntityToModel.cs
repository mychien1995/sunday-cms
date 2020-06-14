using Sunday.Core.Pipelines.Arguments;
using Sunday.VirtualRoles.Core;
using Sunday.VirtualRoles.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.VirtualRoles.Implementation.Pipelines
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

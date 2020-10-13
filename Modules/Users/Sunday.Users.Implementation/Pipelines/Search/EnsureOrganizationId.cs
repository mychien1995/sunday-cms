using Sunday.Core;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Constants;
using Sunday.Core.Context;

namespace Sunday.Users.Implementation.Pipelines.BeforeSearch
{
    public class EnsureOrganizationId
    {
        private readonly ISundayContext _sundayContext;

        public EnsureOrganizationId(ISundayContext sundayContext)
        {
            _sundayContext = sundayContext;
        }
        public async Task ProcessAsync(BeforeSearchUserArg arg)
        {
            if (arg.FinalQuery != null)
            {
                var currentUser = _sundayContext.CurrentUser;
                if (currentUser.IsInRole(SystemRoleCodes.OrganizationAdmin) || currentUser.IsInRole(SystemRoleCodes.OrganizationUser))
                {
                    arg.FinalQuery.OrganizationIds = new List<int>() { _sundayContext.CurrentOrganization.ID };
                }
            }
        }
    }
}

using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users.Search
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

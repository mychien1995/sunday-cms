using Microsoft.AspNetCore.Http;
using Sunday.CMS.Core.Pipelines.Arguments;
using Sunday.Core.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users.Search
{
    public class ExcludeCurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public ExcludeCurrentUser(IHttpContextAccessor httpContext)
        {
            httpContextAccessor = httpContext;
        }
        public async Task ProcessAsync(BeforeSearchUserArg arg)
        {
            if (arg.FinalQuery == null || httpContextAccessor.HttpContext?.User == null) return;
            var currentUser = httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            if (arg.FinalQuery.ExcludeIdList == null) arg.FinalQuery.ExcludeIdList = new List<int>();
            arg.FinalQuery.ExcludeIdList.Add(currentUser.UserId);
        }
    }
}

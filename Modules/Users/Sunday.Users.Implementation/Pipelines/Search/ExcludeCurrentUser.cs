using Microsoft.AspNetCore.Http;
using Sunday.Identity.Core;
using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.BeforeSearch
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

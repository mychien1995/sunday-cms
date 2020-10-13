using Sunday.Core;
using Sunday.Core.Models;
using Sunday.Core.Models.Base;
using Sunday.Users.Core;
using Sunday.Users.Core.Models;

namespace Sunday.Users.Implementation.Pipelines.Arguments
{
    public class AfterSearchUserArg : PipelineArg
    {
        public SearchResult<ApplicationUser> SearchResult { get; set; }
        public UserListJsonResult DisplayResult { get; set; }
        public AfterSearchUserArg()
        {

        }
        public AfterSearchUserArg(SearchResult<ApplicationUser> searchResult, UserListJsonResult userListJsonResult)
        {
            SearchResult = searchResult;
            DisplayResult = userListJsonResult;
        }
    }
}

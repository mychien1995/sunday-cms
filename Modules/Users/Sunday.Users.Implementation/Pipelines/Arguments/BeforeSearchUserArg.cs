using Sunday.Core;
using Sunday.Users.Core.Models;

namespace Sunday.Users.Implementation.Pipelines.Arguments
{
    public class BeforeSearchUserArg : PipelineArg
    {
        public SearchUserCriteria ViewCriteria { get; set; }
        public UserQuery FinalQuery { get; set; }
        public BeforeSearchUserArg()
        {

        }
        public BeforeSearchUserArg(SearchUserCriteria criteria, UserQuery query)
        {
            ViewCriteria = criteria;
            FinalQuery = query;
        }
    }
}

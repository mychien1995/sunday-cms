using Sunday.Users.Implementation.Pipelines.Arguments;
using System.Threading.Tasks;

namespace Sunday.Users.Implementation.Pipelines.BeforeSearch
{
    public class EnsurePageSize
    {
        public async Task ProcessAsync(BeforeSearchUserArg arg)
        {
            if (arg.FinalQuery != null)
            {
                if (arg.FinalQuery.PageIndex == null) arg.FinalQuery.PageIndex = 0;
                if (arg.FinalQuery.PageSize == null || arg.FinalQuery.PageSize > 10) arg.FinalQuery.PageSize = 10;
            }
        }
    }
}

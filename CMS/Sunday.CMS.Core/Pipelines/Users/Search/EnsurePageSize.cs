using Sunday.CMS.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.CMS.Core.Pipelines.Users.Search
{
    public class EnsurePageSize
    {
        public async Task ProcessAsync(BeforeSearchUserArg arg)
        {
            if(arg.FinalQuery != null)
            {
                if (arg.FinalQuery.PageIndex == null) arg.FinalQuery.PageIndex = 0;
                if (arg.FinalQuery.PageSize == null || arg.FinalQuery.PageSize > 10) arg.FinalQuery.PageSize = 10;
            }
        }
    }
}

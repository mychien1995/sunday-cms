using Sunday.CMS.Core.Models.Users;
using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Core.Models;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Pipelines.Arguments
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

using Sunday.CMS.Core.Models.Users;
using Sunday.Core;
using Sunday.Users.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Pipelines.Arguments
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

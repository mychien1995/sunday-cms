using Sunday.Core.Domain.Identity;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Pipelines.Arguments
{
    public class AfterLoggedInArg : PipelineArg
    {
        public ApplicationUser User { get; set; }
        public SignInResult Result { get; set; }
        public string Username { get; set; }
        public AfterLoggedInArg(ApplicationUser user, SignInResult result, string userName)
        {
            User = user;
            Result = result;
            Username = userName;
        }
    }
}

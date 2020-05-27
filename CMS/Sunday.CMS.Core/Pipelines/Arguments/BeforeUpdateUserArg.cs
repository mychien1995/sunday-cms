using Sunday.CMS.Core.Models.Users;
using Sunday.Core;
using Sunday.Core.Domain;
using Sunday.Core.Domain.Users;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Pipelines.Arguments
{
    public class BeforeUpdateUserArg : BeforeCreateUserArg
    {
        public BeforeUpdateUserArg(ApplicationUser user, UserMutationModel createUser) : base(user, createUser)
        {
        }
    }
}

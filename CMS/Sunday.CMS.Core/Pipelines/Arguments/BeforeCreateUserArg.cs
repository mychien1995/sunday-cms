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
    public class BeforeCreateUserArg : PipelineArg, IEntityChangedArg
    {
        public UserMutationModel Input { get; set; }
        public ApplicationUser User { get; set; }
        public IEntity EntityChange
        {
            get
            {
                return User;
            }
            set
            {

            }
        }
        public BeforeCreateUserArg(ApplicationUser user, UserMutationModel createUser)
        {
            this.User = user;
            this.Input = createUser;
        }
    }
}

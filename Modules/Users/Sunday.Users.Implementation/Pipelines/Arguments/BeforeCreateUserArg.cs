using Sunday.Core;
using Sunday.Core.Domain;
using Sunday.Core.Domain.Interfaces;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;
using Sunday.Users.Core;
using Sunday.Users.Core.Models;

namespace Sunday.Users.Implementation.Pipelines.Arguments
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

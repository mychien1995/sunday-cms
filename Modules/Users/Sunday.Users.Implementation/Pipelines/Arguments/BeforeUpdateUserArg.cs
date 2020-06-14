using Sunday.Users.Core;
using Sunday.Users.Core.Models;

namespace Sunday.Users.Implementation.Pipelines.Arguments
{
    public class BeforeUpdateUserArg : BeforeCreateUserArg
    {
        public BeforeUpdateUserArg(ApplicationUser user, UserMutationModel createUser) : base(user, createUser)
        {
        }
    }
}

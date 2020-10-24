using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Implementation.Pipelines.Arguments
{
    public class BeforeCreateUserArg : PipelineArg
    {
        public BeforeCreateUserArg(ApplicationUser user)
        {
            User = user;
        }

        public ApplicationUser User { get; set; }
    }
}

using Sunday.Core.Pipelines;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Implementation.Pipelines.Arguments
{
    public class BeforeUpdateUserArg : PipelineArg
    {
        public BeforeUpdateUserArg(ApplicationUser user)
        {
            User = user;
        }

        public ApplicationUser User { get; set; }
    }
}

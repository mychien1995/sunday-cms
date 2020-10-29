using Sunday.ContentManagement.Domain;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class BeforeCreateWebsiteArg : PipelineArg
    {
        public BeforeCreateWebsiteArg(ApplicationWebsite website)
        {
            Website = website;
        }

        public ApplicationWebsite Website { get; set; }
    }
}

using Sunday.ContentManagement.Domain;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class BeforeUpdateWebsiteArg : PipelineArg
    {
        public BeforeUpdateWebsiteArg(ApplicationWebsite website)
        {
            Website = website;
        }

        public ApplicationWebsite Website { get; set; }
    }
}

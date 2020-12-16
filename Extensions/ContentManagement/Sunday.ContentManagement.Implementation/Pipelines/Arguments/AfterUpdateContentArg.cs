using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class AfterUpdateContentArg : PipelineArg
    {
        public AfterUpdateContentArg(Content content)
        {
            Content = content;
        }

        public Content Content { get; }
    }
}

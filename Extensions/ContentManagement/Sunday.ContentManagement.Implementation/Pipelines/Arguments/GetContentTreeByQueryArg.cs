using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContentTreeByQueryArg : PipelineArg
    {
        public GetContentTreeByQueryArg(ContentTreeQuery contentTreeQuery)
        {
            ContentTreeQuery = contentTreeQuery;
        }

        public ContentTreeQuery ContentTreeQuery { get; set; }
        public ContentTree? ContentTree { get; set; }
    }
}

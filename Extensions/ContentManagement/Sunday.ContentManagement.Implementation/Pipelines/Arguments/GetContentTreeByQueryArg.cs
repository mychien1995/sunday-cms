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

        public bool ExpandLastNode => ContentTreeQuery.ExpandLastNode;
        public ContentTreeQuery ContentTreeQuery { get; }
        public ContentTree? ContentTree { get; set; }
    }
}

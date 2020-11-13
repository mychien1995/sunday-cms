using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContentTreeSnapshotArg : PipelineArg
    {
        public string Path { get; set; }
        public ContentTree ContentTree { get; set; } = new ContentTree();

        public GetContentTreeSnapshotArg(string path)
        {
            Path = path;
        }
    }
}

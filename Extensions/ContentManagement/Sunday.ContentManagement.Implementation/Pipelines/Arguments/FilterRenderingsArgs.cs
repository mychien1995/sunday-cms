using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class FilterRenderingsArgs : PipelineArg
    {
        public RenderingQuery Query { get; }
        public SearchResult<Rendering> Result { get; }

        public FilterRenderingsArgs(RenderingQuery query, SearchResult<Rendering> result)
        {
            Query = query;
            Result = result;
        }
    }
}
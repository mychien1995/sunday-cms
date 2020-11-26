using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class FilterTemplatesArg : PipelineArg
    {
        public TemplateQuery Query { get; }
        public SearchResult<Template> Result { get; }

        public FilterTemplatesArg(TemplateQuery query, SearchResult<Template> result)
        {
            Query = query;
            Result = result;
        }
    }
}

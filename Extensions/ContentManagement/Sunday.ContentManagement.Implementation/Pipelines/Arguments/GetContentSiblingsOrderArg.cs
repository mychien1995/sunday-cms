using System;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContentSiblingsOrderArg : PipelineArg
    {
        public GetContentSiblingsOrderArg(MoveContentParameter moveContentParameter)
        {
            MoveContentParameter = moveContentParameter;
        }

        public MoveContentParameter MoveContentParameter { get; }
        public ContentOrder[] Orders { get; set; } = Array.Empty<ContentOrder>();
    }
}

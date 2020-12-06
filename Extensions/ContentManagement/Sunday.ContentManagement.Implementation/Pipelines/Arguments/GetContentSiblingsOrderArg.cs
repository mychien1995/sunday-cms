using System;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContentSiblingsOrderArg : PipelineArg
    {
        public GetContentSiblingsOrderArg(Content movedContent, Guid targetId, MovePosition position)
        {
            MovedContent = movedContent;
            TargetId = targetId;
            Position = position;
        }

        public Content MovedContent { get;  }
        public Guid TargetId { get; }
        public MovePosition Position { get; }
        public ContentOrder[] Orders { get; set; } = Array.Empty<ContentOrder>();
    }
}

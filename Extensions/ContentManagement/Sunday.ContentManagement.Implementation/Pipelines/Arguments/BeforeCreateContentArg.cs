using System;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class BeforeCreateContentArg : PipelineArg
    {
        public Content Content;

        public BeforeCreateContentArg(Content content)
        {
            Content = content;
        }
    }
    public class BeforeUpdateContentArg : PipelineArg
    {
        public Content Content;

        public BeforeUpdateContentArg(Content content)
        {
            Content = content;
        }
    }
    public class BeforeDeleteContentArg : PipelineArg
    {
        public Guid ContentId;

        public BeforeDeleteContentArg(Guid contentId)
        {
            ContentId = contentId;
        }
    }
}

using System.Collections.Generic;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContentTreeRootArg : PipelineArg
    {
        public List<ContentTreeNode> Roots { get; set; } = new List<ContentTreeNode>();
    }
}

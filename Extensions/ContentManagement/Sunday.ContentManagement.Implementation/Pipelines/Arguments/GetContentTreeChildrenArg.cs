using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Arguments
{
    public class GetContentTreeChildrenArg : PipelineArg
    {
        public GetContentTreeChildrenArg(ContentTreeNode currentNode)
        {
            CurrentNode = currentNode;
        }

        public ContentTreeNode CurrentNode { get; set; }
        public List<ContentTreeNode> ChildNodes { get; set; } = new List<ContentTreeNode>();
    }
}

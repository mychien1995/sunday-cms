using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;
using static Sunday.ContentManagement.Implementation.Constants;
// ReSharper disable StringLiteralTypo

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetWebsiteContextMenu : IPipelineProcessor
    {
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (GetContextMenuArg)pipelineArg;
            var current = arg.CurrentNode;
            if (current.Type == (int)ContentType.Website)
            {
                arg.Menu.Items.Add(new ContextMenuItem
                {
                    Command = "createcontent",
                    Hint = "Create new content under this",
                    Title = "New Content",
                    Icon = "plus_circle"
                });
                arg.Menu.Items.Add(new ContextMenuItem
                {
                    Command = "viewwebsite",
                    Hint = "Preview this website",
                    Title = "Preview",
                    Icon = "eye"
                });
            }
        }
    }
}

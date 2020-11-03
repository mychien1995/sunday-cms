using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.Core.Pipelines;
using static Sunday.ContentManagement.Implementation.Constants;
// ReSharper disable StringLiteralTypo

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetContextMenu : IPipelineProcessor
    {
        public void Process(PipelineArg pipelineArg)
        {
            var arg = (GetContextMenuArg)pipelineArg;
            var current = arg.CurrentNode;
            if (current.Type == NodeTypes.Website)
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
            else if (current.Type == NodeTypes.Content)
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
                    Command = "deletecontent",
                    Hint = "Delete this",
                    Title = "Delete",
                    Icon = "delete_x"
                });
                arg.Menu.Items.Add(new ContextMenuItem
                {
                    Command = "renamecontent",
                    Hint = "Rename this",
                    Title = "Rename",
                    Icon = "eye_dropper"
                });
            }
        }
    }
}

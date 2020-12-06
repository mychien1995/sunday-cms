using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Contents
{
    public class ResolveSiblingsOrder : IAsyncPipelineProcessor
    {
        private readonly IContentService _contentService;

        public ResolveSiblingsOrder(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentSiblingsOrderArg)pipelineArg;
            var content = arg.MovedContent;
            var position = arg.Position;  
            var targetId = arg.TargetId;
            int sortOrder = 0;
            var toSave = new List<ContentOrder>();
            var siblingOpts = arg.PropertyBag.Get("siblings");
            var siblings = siblingOpts.IsSome ? (Content[])siblingOpts.Get() : await _contentService.GetChildsAsync(
                content.ParentId, (ContentType)content.ParentType).MapResultTo(rs => rs.Where(node => node.Id != content.Id).ToArray());
            siblings = siblings.OrderBy(c => c.SortOrder).ToArray();
            if (siblings.Length == 0)
            {
                toSave.Add(new ContentOrder(content.Id, sortOrder));
                return;
            }
            Content? targetNode;
            Content[] lower;
            Content[] higher;
            switch (position)
            {
                case MovePosition.Inside:
                    for (var i = 0; i < siblings.Length; i++)
                    {
                        var current = siblings[i];
                        if (current.Name.ToLower()[0] <= content.DisplayName.ToLower()[0]) continue;
                        if (i < siblings.Length / 2)
                        {
                            sortOrder = siblings[i].SortOrder - 1;
                            var tmpSortOrder = sortOrder;
                            for (var j = i - 1; j > 0; j--)
                            {
                                toSave.Add(new ContentOrder(siblings[j].Id, --tmpSortOrder));
                            }
                        }
                        else
                        {
                            sortOrder = siblings[i].SortOrder;
                            var tmpSortOrder = sortOrder;
                            for (var j = i; j < siblings.Length; j++)
                            {
                                toSave.Add(new ContentOrder(siblings[j].Id, ++tmpSortOrder));
                            }
                        }
                        goto END;
                    }
                    sortOrder = siblings.Max(n => n.SortOrder) + 1;
                    break;
                case MovePosition.Above:
                    targetNode = siblings.Single(n => n.Id == targetId);
                    lower = siblings.Where(n => n.SortOrder < targetNode.SortOrder).ToArray();
                    higher = siblings.Where(n => n.SortOrder >= targetNode.SortOrder).ToArray();
                    if (lower.Length < higher.Length)
                    {
                        sortOrder = targetNode.SortOrder - 1;
                        var tmpSortOrder = sortOrder;
                        lower.Reverse().Iter(node =>
                        {
                            toSave.Add(new ContentOrder(node.Id, --tmpSortOrder));
                        });
                    }
                    else
                    {
                        sortOrder = targetNode.SortOrder;
                        var tmpSortOrder = sortOrder;
                        higher.Iter(node =>
                        {
                            toSave.Add(new ContentOrder(node.Id, ++tmpSortOrder));
                        });
                    }
                    break;
                case MovePosition.Below:
                    targetNode = siblings.Single(n => n.Id == targetId);
                    lower = siblings.Where(n => n.SortOrder <= targetNode.SortOrder).ToArray();
                    higher = siblings.Where(n => n.SortOrder > targetNode.SortOrder).ToArray();
                    if (lower.Length < higher.Length)
                    {
                        sortOrder = targetNode.SortOrder;
                        var tmpSortOrder = sortOrder - 1;
                        lower.Reverse().Iter(node =>
                        {
                            toSave.Add(new ContentOrder(node.Id, --tmpSortOrder));
                        });
                    }
                    else
                    {
                        sortOrder = targetNode.SortOrder + 1;
                        var tmpSortOrder = sortOrder;
                        higher.Iter(node =>
                        {
                            toSave.Add(new ContentOrder(node.Id, ++tmpSortOrder));
                        });
                    }
                    break;
                default: throw new ArgumentException();

            }
            END:
            toSave.Add(new ContentOrder(content.Id, sortOrder));
            arg.Orders = toSave.ToArray();
        }
    }
}

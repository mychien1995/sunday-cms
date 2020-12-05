using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;

namespace Sunday.ContentManagement.Implementation.Pipelines.Contents
{
    public class ResolveSiblingsOrder : IAsyncPipelineProcessor
    {
        private readonly IContentTreeProvider _contentTreeProvider;
        private readonly IContentOrderRepository _contentOrderRepository;

        public ResolveSiblingsOrder(IContentTreeProvider contentTreeProvider, IContentOrderRepository contentOrderRepository)
        {
            _contentTreeProvider = contentTreeProvider;
            _contentOrderRepository = contentOrderRepository;
        }

        public async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentSiblingsOrderArg)pipelineArg;
            var moveContentParameter = arg.MoveContentParameter;
            if (!moveContentParameter.SortOrder.HasValue) return;
            var siblingIds = await _contentTreeProvider.GetChildren(new ContentTreeNode
            {
                Id = moveContentParameter.ParentId!.Value.ToString(),
                Type = moveContentParameter.ParentType!.Value
            }).MapResultTo(rs => rs.Where(node => node.Id != moveContentParameter.ContentId.ToString())
                .Select(node => Guid.Parse(node.Id)).ToArray());
            var contentOrders = await _contentOrderRepository.GetOrders(siblingIds);
            var toSave = new List<ContentOrder>();
            if (contentOrders.Any(c => c.Order == moveContentParameter.SortOrder))
            {
                var greater = contentOrders.Where(c => c.Order >= moveContentParameter.SortOrder).ToList();
                var lower = contentOrders.Where(c => c.Order < moveContentParameter.SortOrder).ToList();
                if (greater.Count > lower.Count)
                {
                    var order = moveContentParameter.SortOrder.Value + 1;
                    greater.Iter(c =>
                    {
                        c.Order = order;
                        order++;
                    });
                    toSave = greater;
                }
                else
                {
                    lower.Iter(c => c.Order--);
                    toSave = lower;
                }
            }
            toSave.Add(new ContentOrder(moveContentParameter.ContentId, moveContentParameter.SortOrder.Value));
            arg.Orders = toSave.ToArray();
        }
    }
}

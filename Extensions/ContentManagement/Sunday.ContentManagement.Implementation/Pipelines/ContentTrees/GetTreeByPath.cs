using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Foundation.Application.Services;
using Sunday.Foundation.Context;

namespace Sunday.ContentManagement.Implementation.Pipelines.ContentTrees
{
    public class GetTreeByPath : BaseGetTreeRootPipelineProcessor
    {
        private readonly IContentPathResolver _contentPathResolver;
        private readonly IContentService _contentService;

        public GetTreeByPath(IContentPathResolver contentPathResolver, IWebsiteService websiteService, IOrganizationService organizationService,
            IContentService contentService, ISundayContext sundayContext) :
            base(sundayContext, organizationService, websiteService)
        {
            _contentPathResolver = contentPathResolver;
            _contentService = contentService;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeSnapshotArg)pipelineArg;
            var path = arg.Path;
            var tree = await GetTreeRoot();
            var addressOpt = await _contentPathResolver.GetAddressByPath(path);
            if (addressOpt.IsNone) return;
            var address = addressOpt.Get();
            var orgNode = tree.Roots.FirstOrDefault(n => n.Id == address.Organization!.Id.ToString())!;
            var websiteNode = orgNode.ChildNodes.FirstOrDefault(n => n.Id == address.Website!.Id.ToString())!;
            websiteNode.Open = true;
            var contents = await _contentService.GetChildsAsync(address.Website!.Id, ContentType.Website);
            contents.Iter(content =>
            {
                websiteNode.ChildNodes.Add(FromContent(content));
            });
            var currentNode = websiteNode;
            var ancestors = new Queue<Content>(address.Ancestors.Rev());
            while (ancestors.Count > 0)
            {
                var content = ancestors.Dequeue();
                var node = currentNode.ChildNodes.FirstOrDefault(n => n.Id == content.Id.ToString());
                if (node == null || ancestors.Count == 0) break;
                var childContents = await _contentService.GetChildsAsync(Guid.Parse(node.Id), ContentType.Content);
                childContents.Iter(childContent =>
                {
                    node.ChildNodes.Add(FromContent(childContent));
                });
                currentNode = node;
                currentNode.Open = true;
            }
        }
    }
}

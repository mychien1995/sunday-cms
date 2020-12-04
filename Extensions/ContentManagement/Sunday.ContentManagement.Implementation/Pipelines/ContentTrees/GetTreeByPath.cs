﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunday.ContentManagement.Implementation.Pipelines.Arguments;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
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

        public GetTreeByPath(ISundayContext sundayContext, IOrganizationService organizationService, IWebsiteService websiteService,
            IContentPathResolver contentPathResolver, IContentService contentService, IContentOrderRepository contentOrderRepository)
            : base(sundayContext, organizationService, websiteService, contentOrderRepository, contentService)
        {
            _contentPathResolver = contentPathResolver;
        }

        public override async Task ProcessAsync(PipelineArg pipelineArg)
        {
            var arg = (GetContentTreeSnapshotArg)pipelineArg;
            var path = arg.Path;
            var tree = await GetTreeRoot();
            arg.ContentTree = tree;
            var addressOpt = await _contentPathResolver.GetAddressByPath(path);
            if (addressOpt.IsNone) return;
            var address = addressOpt.Get();
            var orgNode = tree.Roots.FirstOrDefault(n => n.Id == address.Organization!.Id.ToString())!;
            orgNode.Open = true;
            var websiteNode = orgNode.ChildNodes.FirstOrDefault(n => n.Id == address.Website!.Id.ToString())!;
            websiteNode.Open = true;
            websiteNode.ParentId = orgNode.Id;
            var contents = await GetContentChilds(address.Website!.Id, ContentType.Website);
            contents.Iter(content =>
            {
                websiteNode.ChildNodes.Add(FromContent(content, websiteNode.Id));
            });
            var currentNode = websiteNode;
            var ancestors = new Queue<Content>(address.Ancestors);
            while (ancestors.Count > 0)
            {
                var content = ancestors.Dequeue();
                var node = currentNode.ChildNodes.FirstOrDefault(n => n.Id == content.Id.ToString());
                if (node == null) break;
                if (ancestors.Count == 0)
                {
                    currentNode.Open = true;
                    break;
                }
                var childContents = await GetContentChilds(Guid.Parse(node.Id), ContentType.Content);
                childContents.Iter(childContent =>
                {
                    node.ChildNodes.Add(FromContent(childContent, node.Id));
                });
                currentNode = node;
                currentNode.Open = true;
            }
        }
    }
}

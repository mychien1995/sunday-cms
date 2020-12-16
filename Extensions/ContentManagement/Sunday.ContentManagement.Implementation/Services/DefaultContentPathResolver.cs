using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentPathResolver))]
    public class DefaultContentPathResolver : IContentPathResolver
    {

        private readonly IWebsiteService _websiteService;
        private readonly IOrganizationService _organizationService;
        private readonly IContentService _contentService;

        public DefaultContentPathResolver(IContentService contentService, IOrganizationService organizationService, IWebsiteService websiteService)
        {
            _contentService = contentService;
            _organizationService = organizationService;
            _websiteService = websiteService;
        }

        public string GetWebsitePath(ApplicationWebsite website)
            => $"{website.OrganizationId}/{website.Id}";
        public async Task<ContentAddress> GetAncestors(Content content)
        {
            var emptyAddress = new ContentAddress();
            await TraverseAddress(content, emptyAddress);
            if (emptyAddress.Website == null || emptyAddress.Organization == null)
                throw new ArgumentException("Website and Organization not found");
            return emptyAddress;
            async Task TraverseAddress(Content currentContent, ContentAddress address)
            {
                var stack = new Stack<Content>();
                stack.Push(currentContent);
                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    address.Ancestors.Insert(0, current);
                    if (current.ParentType == (int)ContentType.Website)
                    {
                        var website = await _websiteService.GetByIdAsync(current.ParentId).MapResultTo(rs => rs.Get());
                        var organization = await _organizationService.GetOrganizationByIdAsync(website.OrganizationId)
                            .MapResultTo(rs => rs.Get());
                        address.Website = website;
                        address.Organization = organization;
                        break;
                    }
                    stack.Push(await _contentService.GetByIdAsync(current.ParentId).MapResultTo(rs => rs.Get()));
                }
            }
        }

        public async Task<Option<ContentAddress>> GetAddressByPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return Option<ContentAddress>.None;
            var clonedPath = path.Trim('/');
            var parts = clonedPath.Split('/').Select(p => p.Trim()).ToList();
            if (parts.Count < 3 || parts.Any(p => !Guid.TryParse(p, out _))) return Option<ContentAddress>.None;
            var organizationOpt = await _organizationService.GetOrganizationByIdAsync(Guid.Parse(parts[0]));
            if (organizationOpt.IsNone) return Option<ContentAddress>.None;
            var websiteOpt = await _websiteService.GetByIdAsync(Guid.Parse(parts[1]));
            if (websiteOpt.IsNone || websiteOpt.Get().OrganizationId != organizationOpt.Get().Id) return Option<ContentAddress>.None;
            parts.RemoveAt(0);
            parts.RemoveAt(0);
            var address = new ContentAddress { Organization = organizationOpt.Get(), Website = websiteOpt.Get() };
            var parentId = websiteOpt.Get().Id;
            while (parts.Count > 0)
            {
                var currentId = Guid.Parse(parts[0]);
                var content = await _contentService.GetByIdAsync(currentId);
                if (content.IsNone || content.Get().ParentId != parentId) return Option<ContentAddress>.None;
                address.Ancestors.Add(content.Get());
                parentId = content.Get().Id;
                parts.RemoveAt(0);
            }
            return address;
        }

        public async Task<Option<Content>> GetContentByNamePath(Guid websiteId, string path)
        {
            var formalizedPath = path.FormalizeAsContentPath();
            if (string.IsNullOrEmpty(formalizedPath)) return Option<Content>.None;
            var parts = formalizedPath.Split('/').Select(p => p.Trim()).ToList();
            var currentContent = Option<Content>.None;
            var currentId = websiteId;
            var contentType = ContentType.Website;
            while (parts.Count > 0)
            {
                var currentName = parts[0];
                var childs = await _contentService.GetChildsAsync(currentId, contentType);
                currentContent = Optional(childs.FirstOrDefault(c => c.Name.ToLower() == currentName));
                if (currentContent.IsNone) return currentContent;
                parts.RemoveAt(0);
                contentType = ContentType.Content;
            }
            return currentContent;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Foundation.Application.Services;

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

        public async Task<Option<Content>> GetContentByNamePath(Guid websiteId, string path, bool includeHome = false)
        {
            var formalizedPath = path.Trim().Trim('/').ToLower().Replace('\\', '/');
            var roots = await _contentService.GetChildsAsync(websiteId, ContentType.Website);
            if (!roots.Any()) return Option<Content>.None;
            var home = roots.FirstOrDefault(IsPage);
            if (home == null) return Option<Content>.None;
            if (string.IsNullOrEmpty(formalizedPath)) return home;
            var parts = formalizedPath.Split('/').Select(p => p.Trim()).ToList();
            if (includeHome)
            {
                var homePart = parts[0];
                if (home.Name.ToLower() == homePart) return home;
                parts.RemoveAt(0);
            }
            var currentContent = home;
            while (parts.Count > 0)
            {
                var currentName = parts[0];
                var childs = await _contentService.GetChildsAsync(currentContent.Id, ContentType.Content);
                currentContent = childs.FirstOrDefault(c => c.Name.ToLower() == currentName);
                if (currentContent == null) return Option<Content>.None;
                parts.RemoveAt(0);
            }
            return currentContent;
        }
        private bool IsPage(Content content) => true;
    }
}

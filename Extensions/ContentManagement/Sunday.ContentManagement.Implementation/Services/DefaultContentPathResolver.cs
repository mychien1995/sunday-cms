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
            emptyAddress.Ancestors.Add(content);
            return emptyAddress;
            async Task TraverseAddress(Content currentContent, ContentAddress address)
            {
                var stack = new Stack<Content>();
                stack.Push(currentContent);
                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    if (current.ParentType == (int)ContentType.Website)
                    {
                        var website = await _websiteService.GetByIdAsync(currentContent.ParentId).MapResultTo(rs => rs.Get());
                        var organization = await _organizationService.GetOrganizationByIdAsync(website.OrganizationId)
                            .MapResultTo(rs => rs.Get());
                        address.Website = website;
                        address.Organization = organization;
                        break;
                    }
                    _ = address.Ancestors.Prepend(current);
                    stack.Push(await _contentService.GetByIdAsync(current.ParentId).MapResultTo(rs => rs.Get()));
                }
            }
        }

        public Task<Option<ContentAddress>> GetAddressByPath(string path)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Models;
using Sunday.Core;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentReader), LifetimeScope.Singleton)]
    public class SwitchableContentReader : IContentReader
    {
        private readonly IContentReader _innerContentReader;

        public SwitchableContentReader(IServiceProvider serviceProvider)
        {
            var showUnpublished = ApplicationSettings.Get<bool>("Sunday.CD.ShowUnpublished");
            if (!showUnpublished)
            {
                _innerContentReader = ActivatorUtilities.CreateInstance<DefaultContentReader>(serviceProvider);
            }
            else
            {
                _innerContentReader = ActivatorUtilities.CreateInstance<UnpublishedContentReader>(serviceProvider);
            }

        }

        public Task<Option<Content>> GetHomePage(Guid websiteId)
            => _innerContentReader.GetHomePage(websiteId);

        public Task<Option<Content>> GetPage(Guid websiteId, string path)
            => _innerContentReader.GetPage(websiteId, path);

        public Task<Option<Content>> GetContent(Guid contentId)
            => _innerContentReader.GetContent(contentId);

        public Task<Content[]> GetChilds(Guid parentId, ContentType contentType)
            => _innerContentReader.GetChilds(parentId, contentType);
    }
}

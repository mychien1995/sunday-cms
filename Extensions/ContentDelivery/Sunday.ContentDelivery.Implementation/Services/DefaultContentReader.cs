using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(IContentReader))]
    public class DefaultContentReader : IContentReader
    {
        private readonly IContentPathResolver _contentPathResolver;

        public DefaultContentReader(IContentPathResolver contentPathResolver)
        {
            _contentPathResolver = contentPathResolver;
        }


        public Task<Option<Content>> GetContentByNamePath(Guid websiteId, string path)
            => _contentPathResolver.GetContentByNamePath(websiteId, path);
    }
}

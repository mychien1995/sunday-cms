using System;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Services;
using Sunday.Core;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(IRenderingReader))]
    public class DefaultRenderingReader : IRenderingReader
    {
        private readonly IRenderingService _renderingService;

        public DefaultRenderingReader(IRenderingService renderingService)
        {
            _renderingService = renderingService;
        }

        public Task<Option<Rendering>> GetRendering(Guid id)
            => _renderingService.GetRenderingById(id);
    }
}

using System;
using System.Threading.Tasks;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Services;
using Sunday.Core;
using Sunday.Core.Extensions;

namespace Sunday.ContentDelivery.Implementation.Services
{
    [ServiceTypeOf(typeof(ILayoutReader))]
    public class DefaultLayoutReader : ILayoutReader
    {
        private readonly ILayoutService _layoutService;

        public DefaultLayoutReader(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public Task<string?> GetLayout(ApplicationWebsite website)
            => _layoutService.GetByIdAsync(website.LayoutId).MapResultTo(rs => rs.IsSome ? (string?)rs.Get().LayoutPath : null);
    }
}

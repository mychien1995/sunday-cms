using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Persistence.Models
{
    [MappedTo(typeof(RenderingQuery))]
    public class RenderingQueryParameter : PagingCriteria
    {
        public string? Text { get; set; }
        public bool? IsPageRendering { get; set; }
    }
}

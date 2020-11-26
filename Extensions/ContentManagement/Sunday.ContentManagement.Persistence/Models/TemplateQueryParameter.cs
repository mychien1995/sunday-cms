using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Persistence.Models
{
    [MappedTo(typeof(TemplateQuery))]
    public class TemplateQueryParameter : PagingCriteria
    {
        public bool? IsAbstract { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}

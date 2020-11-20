using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Models
{
    public class RenderingQuery : PagingCriteria
    {
        public string? Text { get; set; }
    }
}

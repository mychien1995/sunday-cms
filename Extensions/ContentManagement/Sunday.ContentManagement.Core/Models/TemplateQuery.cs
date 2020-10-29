using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Models
{
    public class TemplateQuery : PagingCriteria
    {
        public string Text { get; set; } = string.Empty;
    }
}

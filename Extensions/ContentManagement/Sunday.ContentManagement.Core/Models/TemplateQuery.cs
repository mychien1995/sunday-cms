using System;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Models
{
    public class TemplateQuery : PagingCriteria
    {
        public bool? IsAbstract { get; set; }
        public bool? IsPageTemplate { get; set; }
        public Guid? WebsiteId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string[] IncludeIds { get; set; } = Array.Empty<string>();
    }
}

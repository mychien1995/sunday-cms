using System;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Models
{
    public class RenderingQuery : PagingCriteria
    {
        public string? Text { get; set; }
        public Guid? WebsiteId { get; set; }
        public Guid? OrganizationId { get; set; }
        public bool? IsPageRendering { get; set; }
    }
}

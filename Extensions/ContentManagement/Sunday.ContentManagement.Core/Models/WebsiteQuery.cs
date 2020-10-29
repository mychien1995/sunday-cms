using System;
using Sunday.Core.Models.Base;

namespace Sunday.ContentManagement.Models
{
    public class WebsiteQuery : PagingCriteria
    {
        public Guid? OrganizationId { get; set; }
    }
}

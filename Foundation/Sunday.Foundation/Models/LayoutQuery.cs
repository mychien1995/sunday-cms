using System;
using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class LayoutQuery : PagingCriteria
    {
        public Guid? LayoutId { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}

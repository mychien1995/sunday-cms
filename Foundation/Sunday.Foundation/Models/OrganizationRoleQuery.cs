using System;
using Sunday.Core.Models.Base;

namespace Sunday.Foundation.Models
{
    public class OrganizationRoleQuery : PagingCriteria
    {
        public Guid OrganizationId { get; set; }
    }
}

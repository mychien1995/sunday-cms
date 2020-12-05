using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Organizations
{
    public class OrganizationListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<OrganizationItem> Organizations { get; set; } = new List<OrganizationItem>();
    }

    [MappedTo(typeof(ApplicationOrganization))]
    public class OrganizationItem
    {
        public Guid Id { get; set; }
        public string LogoLink { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public long CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public long UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; } = string.Empty;
    }
}

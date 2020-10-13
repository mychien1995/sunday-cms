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
        public OrganizationItem(Guid id, string logoLink, string organizationName, string createdBy, long createdDate,
            string updatedBy, long updatedDate, bool isActive, string colorScheme)
        {
            Id = id;
            LogoLink = logoLink;
            OrganizationName = organizationName;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            IsActive = isActive;
            ColorScheme = colorScheme;
        }

        public Guid Id { get; set; }
        public string LogoLink { get; set; }
        public string OrganizationName { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public long UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ColorScheme { get; set; }
    }
}

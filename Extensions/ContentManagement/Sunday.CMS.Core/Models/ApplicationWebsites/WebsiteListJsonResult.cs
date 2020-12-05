using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Domain;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.ApplicationWebsites
{
    public class WebsiteListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<WebsiteItem> Websites { get; set; } = new List<WebsiteItem>();
    }

    [MappedTo(typeof(ApplicationWebsite))]
    public class WebsiteItem
    {
        public Guid Id { get; set; }
        public string WebsiteName { get; set; } = string.Empty;
        public string[] HostNames { get; set; } = Array.Empty<string>();
        public Guid LayoutId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public Guid OrganizationId { get; set; }
    }
}

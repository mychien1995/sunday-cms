using System;
using System.Collections.Generic;
using Sunday.ContentManagement.Domain;
using Sunday.Core;

namespace Sunday.CMS.Core.Models.ApplicationWebsites
{
    [MappedTo(typeof(ApplicationWebsite))]
    public class WebsiteMutationModel
    {
        public Guid? Id { get; set; }
        public string WebsiteName { get; set; } = string.Empty;
        public string[] HostNames { get; set; } = Array.Empty<string>();
        public Guid LayoutId { get; set; }
        public bool IsActive { get; set; }
        public Guid? OrganizationId { get; set; }
        public Dictionary<string, string> PageDesignMappings { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}

﻿using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Domain
{
    public class ApplicationWebsite : IEntity
    {
        public Guid Id { get; set; }
        public string WebsiteName { get; set; } = string.Empty;
        public string[] HostNames { get; set; } = Array.Empty<string>();
        public Guid OrganizationId { get; set; }
        public Guid LayoutId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public Dictionary<string, string> PageDesignMappings { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}

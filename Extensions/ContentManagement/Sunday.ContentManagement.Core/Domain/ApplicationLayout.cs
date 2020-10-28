using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Domain
{
    public class ApplicationLayout : IEntity
    {
        public Guid Id { get; set; }
        public string LayoutName { get; set; } = string.Empty;
        public string LayoutPath { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
    }
}

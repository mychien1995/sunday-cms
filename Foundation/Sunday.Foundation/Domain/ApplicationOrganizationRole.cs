using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganizationRole : IEntity
    {

        public Guid Id { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public Guid OrganizationId { get; set; }
        public ApplicationOrganization? Organization { get; set; }
        public List<ApplicationFeature> Features { get; set; } = new List<ApplicationFeature>();
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}

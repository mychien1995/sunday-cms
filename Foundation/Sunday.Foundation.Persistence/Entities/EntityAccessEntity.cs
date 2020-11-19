using System;

namespace Sunday.Foundation.Persistence.Entities
{
    public class EntityAccessEntity
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public Guid OrganizationId { get; set; }
        public string WebsiteIds { get; set; } = string.Empty;
        public string OrganizationRoleIds { get; set; } = string.Empty;
    }
}

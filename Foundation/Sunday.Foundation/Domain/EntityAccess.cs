using System;

namespace Sunday.Foundation.Domain
{
    public class EntityAccess
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } = string.Empty;

        public EntityOrganizationAccess[] OrganizationAccess { get; set; } = Array.Empty<EntityOrganizationAccess>();
    }

    public class EntityOrganizationAccess
    {
        public Guid OrganizationId { get; set; }
        public Guid[] WebsiteIds { get; set; } = Array.Empty<Guid>();
        public Guid[] OrganizationRoleIds { get; set; } = Array.Empty<Guid>();
    }
}

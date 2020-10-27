using System;

namespace Sunday.Foundation.Persistence.Entities
{
    public class OrganizationUserRoleEntity
    {
        public Guid OrganizationUserId { get; set; }
        public Guid OrganizationRoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}

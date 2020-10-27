using System;

namespace Sunday.Foundation.Persistence.Entities
{
    public class UserRoleEntity
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}

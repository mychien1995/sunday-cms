using System;

namespace Sunday.Foundation.Persistence.Entities
{
    public class RoleEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }

        public RoleEntity(Guid id, string code, string roleName)
        {
            Id = id;
            Code = code;
            RoleName = roleName;
        }
    }
}

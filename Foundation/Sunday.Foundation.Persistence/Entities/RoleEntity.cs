using System;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationRole))]
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;

        public RoleEntity()
        {
            
        }

        public RoleEntity(int id, string code, string name)
        {
            Id = id;
            Code = code;
            RoleName = name;
        }
    }
}

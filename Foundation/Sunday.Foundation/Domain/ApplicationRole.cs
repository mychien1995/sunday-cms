using System;

namespace Sunday.Foundation.Domain
{
    public class ApplicationRole
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }

        public ApplicationRole(Guid id, string code, string roleName)
        {
            Id = id;
            Code = code;
            RoleName = roleName;
        }
    }
}

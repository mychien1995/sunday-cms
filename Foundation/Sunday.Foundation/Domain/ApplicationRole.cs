using System;

namespace Sunday.Foundation.Domain
{
    public class ApplicationRole
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }

        public ApplicationRole(int id, string code, string roleName)
        {
            Id = id;
            Code = code;
            RoleName = roleName;
        }
    }
}

using Sunday.Core.Domain.Roles;

namespace Sunday.Users.Core
{
    public class ApplicationRole : IApplicationRole
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}

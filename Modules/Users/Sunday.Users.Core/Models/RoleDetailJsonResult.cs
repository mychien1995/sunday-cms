using Sunday.Core;

namespace Sunday.Users.Core.Models
{
    [MappedTo(typeof(ApplicationRole))]
    public class RoleDetailJsonResult : BaseApiResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }

        public bool RequireOrganization { get; set; }
    }
}

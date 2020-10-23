using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.Users.Core.Models
{
    public class RoleListJsonResult : ListApiResponse<RoleItem>
    {
        public RoleListJsonResult() : base()
        {
        }
    }

    [MappedTo(typeof(ApplicationRole))]
    public class RoleItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }

        public bool RequireOrganization { get; set; }
    }
}

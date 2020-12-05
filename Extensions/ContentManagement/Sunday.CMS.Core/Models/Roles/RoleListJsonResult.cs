using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Roles
{
    public class RoleListJsonResult : ListApiResponse<RoleItem>
    {
    }

    [MappedTo(typeof(ApplicationRole))]
    public class RoleItem
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;

        public bool RequireOrganization { get; set; }
    }
}

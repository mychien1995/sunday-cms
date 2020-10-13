using Sunday.Core;
using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.Users.Core.Models
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserDetailJsonResult : BaseApiResponse
    {
        public UserDetailJsonResult()
        {
            RoleIds = new List<int>();
            Organizations = new List<OrganizationsUserItem>();
            OrganizationRoleIds = new List<int>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }
        public List<OrganizationsUserItem> Organizations { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> OrganizationRoleIds { get; set; }
    }

    public class OrganizationsUserItem
    {
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}

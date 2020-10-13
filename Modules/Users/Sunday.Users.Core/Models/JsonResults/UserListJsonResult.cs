using Sunday.Core;
using Sunday.VirtualRoles.Core;
using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.Users.Core.Models
{
    public class UserListJsonResult : BaseApiResponse
    {
        public UserListJsonResult()
        {
            Users = new List<UserItem>();
        }
        public int Total { get; set; }
        public List<UserItem> Users { get; set; }
    }

    [MappedTo(typeof(ApplicationUser))]
    public class UserItem
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Domain { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockedOut { get; set; }
        public List<ApplicationRole> Roles { get; set; }
        public List<OrganizationRole> OrganizationRoles { get; set; }
        public UserItem()
        {
            Roles = new List<ApplicationRole>();
            OrganizationRoles = new List<OrganizationRole>();
        }
    }
}

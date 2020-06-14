using Sunday.Core;
using System.Collections.Generic;

namespace Sunday.Users.Core.Models
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserMutationModel
    {
        public UserMutationModel()
        {
            RoleIds = new List<int>();
            OrganizationRoleIds = new List<int>();
            Organizations = new List<OrganizationSelectionModel>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }
        public string AvatarBlobUri { get; set; }

        public List<int> RoleIds { get; set; }

        public List<int> OrganizationRoleIds { get; set; }
        public List<OrganizationSelectionModel> Organizations { get; set; }
    }

    public class OrganizationSelectionModel
    {
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}

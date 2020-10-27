using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Users
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserMutationModel
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Domain { get; set; } = string.Empty;
        public string AvatarBlobUri { get; set; } = string.Empty;

        public List<int> RoleIds { get; set; } = new List<int>();

        public List<Guid> OrganizationRoleIds { get; set; } = new List<Guid>();
        public List<OrganizationSelectionModel> Organizations { get; set; } = new List<OrganizationSelectionModel>();
    }

    public class OrganizationSelectionModel
    {
        public Guid OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}

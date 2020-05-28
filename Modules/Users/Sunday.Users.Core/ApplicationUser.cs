using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Roles;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Users.Core
{
    public class ApplicationUser : IApplicationUser
    {
        public ApplicationUser()
        {
            Roles = new List<IApplicationRole>();
            OrganizationUsers = new List<IApplicationOrganizationUser>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLockedOut { get; set; }
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AvatarBlobUri { get; set; }
        public string Fullname { get; set; }
        public List<IApplicationRole> Roles { get; set; }
        public List<IApplicationOrganizationUser> OrganizationUsers { get; set; }
    }
}

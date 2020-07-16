using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunday.Foundation.Domain
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            Roles = new List<ApplicationRole>();
            OrganizationUsers = new List<ApplicationOrganizationUser>();
            VirtualRoles = new List<ApplicationOrganizationRole>();
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
        public List<ApplicationRole> Roles { get; set; }
        public List<ApplicationOrganizationRole> VirtualRoles { get; set; }
        public List<ApplicationOrganizationUser> OrganizationUsers { get; set; }

        public bool IsInRole(string roleCode)
        {
            if (Roles == null || !Roles.Any()) return false;
            return Roles.Any(c => c.Code == roleCode);
        }
    }
}

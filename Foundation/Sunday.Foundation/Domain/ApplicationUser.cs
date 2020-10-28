using System;
using System.Collections.Generic;
using System.Linq;
using Sunday.Core.Constants;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.Foundation.Domain
{
    public class ApplicationUser : IEntity
    {

        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLockedOut { get; set; }
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? AvatarBlobUri { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public List<ApplicationOrganizationRole> VirtualRoles { get; set; } = new List<ApplicationOrganizationRole>();
        public List<ApplicationOrganizationUser> OrganizationUsers { get; set; } = new List<ApplicationOrganizationUser>();

        public bool IsInRole(string roleCode)
        {
            if (Roles == null || !Roles.Any()) return false;
            return Roles.Any(c => c.Code == roleCode);
        }

        public bool IsOrganizationMember()
            => IsInRole(SystemRoleCodes.OrganizationAdmin) || IsInRole(SystemRoleCodes.OrganizationUser);
    }
}

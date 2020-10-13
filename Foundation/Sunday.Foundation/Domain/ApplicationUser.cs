using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunday.Foundation.Domain
{
    public class ApplicationUser
    {
        public ApplicationUser(Guid id, string userName, DateTime createdDate, DateTime updatedDate, string createdBy, 
            string updatedBy, bool isActive, bool isDeleted, bool isLockedOut, bool emailConfirmed, 
            string securityStamp, string passwordHash, string domain, string email, string? phone, string? avatarBlobUri, string fullname)
        {
            Id = id;
            UserName = userName;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            IsActive = isActive;
            IsDeleted = isDeleted;
            IsLockedOut = isLockedOut;
            EmailConfirmed = emailConfirmed;
            SecurityStamp = securityStamp;
            PasswordHash = passwordHash;
            Domain = domain;
            Email = email;
            Phone = phone;
            AvatarBlobUri = avatarBlobUri;
            Fullname = fullname;
        }

        public Guid Id { get; set; }
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
        public string? Phone { get; set; }
        public string? AvatarBlobUri { get; set; }
        public string Fullname { get; set; }
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public List<ApplicationOrganizationRole> VirtualRoles { get; set; } = new List<ApplicationOrganizationRole>();
        public List<ApplicationOrganizationUser> OrganizationUsers { get; set; } = new List<ApplicationOrganizationUser>();

        public bool IsInRole(string roleCode)
        {
            if (Roles == null || !Roles.Any()) return false;
            return Roles.Any(c => c.Code == roleCode);
        }
    }
}

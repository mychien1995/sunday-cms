using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserEntity
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
        public string PasswordHash { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? AvatarBlobUri { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public List<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
        public List<OrganizationRoleEntity> VirtualRoles { get; set; } = new List<OrganizationRoleEntity>();
        public List<OrganizationUserEntity> OrganizationUsers { get; set; } = new List<OrganizationUserEntity>();

    }
}

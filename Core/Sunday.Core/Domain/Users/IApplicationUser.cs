using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Users
{
    public interface IApplicationUser : IEntity
    {
        public string UserName { get; set; }
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

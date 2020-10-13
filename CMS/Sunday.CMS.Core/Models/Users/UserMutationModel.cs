using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Users
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserMutationModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }
        public string AvatarBlobUri { get; set; }

        public List<int> RoleIds { get; set; } = new List<int>();

        public List<int> OrganizationRoleIds { get; set; } = new List<int>();
        public List<OrganizationSelectionModel> Organizations { get; set; } = new List<OrganizationSelectionModel>();

        public UserMutationModel(Guid id, string userName, string password, string fullname, string email, string phone,
            bool isActive, string domain, string avatarBlobUri)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Fullname = fullname;
            Email = email;
            Phone = phone;
            IsActive = isActive;
            Domain = domain;
            AvatarBlobUri = avatarBlobUri;
        }
    }

    public class OrganizationSelectionModel
    {
        public Guid OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}

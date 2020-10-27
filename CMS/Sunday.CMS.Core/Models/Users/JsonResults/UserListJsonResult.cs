using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Users.JsonResults
{
    public class UserListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<UserItem> Users { get; set; } = new List<UserItem>();
    }

    [MappedTo(typeof(ApplicationUser))]
    public class UserItem
    {
        public UserItem(Guid id, string userName, string email, string phone, 
            string domain, string fullname, bool isActive, bool isLockedOut)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Phone = phone;
            Domain = domain;
            Fullname = fullname;
            IsActive = isActive;
            IsLockedOut = isLockedOut;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Domain { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockedOut { get; set; }
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
        public List<ApplicationOrganizationRole> OrganizationRoles { get; set; } = new List<ApplicationOrganizationRole>();
    }
}

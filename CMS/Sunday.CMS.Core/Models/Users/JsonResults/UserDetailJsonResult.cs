using System;
using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Users.JsonResults
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserDetailJsonResult : BaseApiResponse
    {
        public UserDetailJsonResult(Guid id, string userName, string fullname,
            string email, string phone, bool isActive, string domain)
        {
            Id = id;
            UserName = userName;
            Fullname = fullname;
            Email = email;
            Phone = phone;
            IsActive = isActive;
            Domain = domain;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }
        public List<OrganizationsUserItem> Organizations { get; set; } = new List<OrganizationsUserItem>();
        public List<int> RoleIds { get; set; } = new List<int>();
        public List<int> OrganizationRoleIds { get; set; } = new List<int>();
    }

    public class OrganizationsUserItem
    {
        public OrganizationsUserItem(string organizationName, int organizationId, bool isActive)
        {
            OrganizationName = organizationName;
            OrganizationId = organizationId;
            IsActive = isActive;
        }

        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
    }
}

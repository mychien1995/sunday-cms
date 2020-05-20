using Sunday.Core;
using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Users
{
    public class UserListJsonResult : BaseApiResponse
    {
        public UserListJsonResult()
        {
            Users = new List<UserItem>();
        }
        public int Total { get; set; }
        public IEnumerable<UserItem> Users { get; set; }
    }

    [MappedTo(typeof(ApplicationUser))]
    public class UserItem
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLockedOut { get; set; }
        public UserItem()
        {

        }
    }
}

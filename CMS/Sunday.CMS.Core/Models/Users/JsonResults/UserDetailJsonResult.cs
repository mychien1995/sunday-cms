using Sunday.Core;
using Sunday.Core.Domain.Users;
using Sunday.Users.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Users
{
    [MappedTo(typeof(ApplicationUser))]
    public class UserDetailJsonResult : BaseApiResponse
    {
        public UserDetailJsonResult()
        {
            RoleIds = new List<int>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }

        public List<int> RoleIds { get; set; }
    }
}

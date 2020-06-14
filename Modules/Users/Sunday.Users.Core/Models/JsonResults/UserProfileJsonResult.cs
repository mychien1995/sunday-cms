using Sunday.Core;
using System.Collections.Generic;

namespace Sunday.Users.Core.Models
{
    [MappedTo(typeof(UserDetailJsonResult))]
    public class UserProfileJsonResult : BaseApiResponse
    {
        public UserProfileJsonResult()
        {
            RoleIds = new List<int>();
        }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }

        public List<int> RoleIds { get; set; }
    }
}

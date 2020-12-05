using System.Collections.Generic;
using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Users.JsonResults
{
    [MappedTo(typeof(UserDetailJsonResult))]
    public class UserProfileJsonResult : BaseApiResponse
    {
        public UserProfileJsonResult()
        {
            RoleIds = new List<int>();
        }

        public string UserName { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; } = string.Empty;

        public List<int> RoleIds { get; set; }
    }
}

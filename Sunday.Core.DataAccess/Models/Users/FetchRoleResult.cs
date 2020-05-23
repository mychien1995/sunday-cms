using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.DataAccess.Models.Users
{
    public class FetchRoleResult
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}

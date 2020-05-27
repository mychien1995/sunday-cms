using Sunday.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Users.Core
{
    public class ApplicationRole : IApplicationRole
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}

using Sunday.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Identity
{
    public interface IApplicationUserPrincipal
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public List<IApplicationRole> Roles { get; set; }
    }
}

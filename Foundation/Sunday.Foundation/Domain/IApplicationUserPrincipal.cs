using System;
using System.Collections.Generic;

namespace Sunday.Foundation.Domain
{
    public interface IApplicationUserPrincipal
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public List<ApplicationRole> Roles { get; set; }
    }
}

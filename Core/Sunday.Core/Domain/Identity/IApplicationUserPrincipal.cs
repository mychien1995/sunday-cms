using Sunday.Core.Domain.Roles;
using System.Collections.Generic;

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

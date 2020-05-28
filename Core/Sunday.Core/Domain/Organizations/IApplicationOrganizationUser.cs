using Sunday.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Domain.Organizations
{
    public interface IApplicationOrganizationUser
    {
        public IApplicationOrganization Organization { get; set; }
        public IApplicationUser User { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int ID { get; set; }
        public bool IsActive { get; set; }
    }
}

using Sunday.Core.Domain.Organizations;
using Sunday.Core.Domain.Users;

namespace Sunday.Organizations.Core
{
    public class ApplicationOrganizationUser : IApplicationOrganizationUser
    {
        public IApplicationOrganization Organization { get; set; }
        public virtual IApplicationUser User { get; set; }
        public virtual int OrganizationId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsActive { get; set; }
    }
}

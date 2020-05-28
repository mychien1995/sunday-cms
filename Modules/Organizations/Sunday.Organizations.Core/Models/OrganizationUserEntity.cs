using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Organizations.Core.Models
{
    [MappedTo(new Type[] { typeof(ApplicationOrganizationUser), typeof(IApplicationOrganizationUser) })]
    public class OrganizationUserEntity
    {
        public virtual string OrganizationName { get; set; }
        public virtual int OrganizationId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsActive { get; set; }
    }
}

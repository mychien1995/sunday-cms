using System;
using Sunday.Core;
using Sunday.Core.Domain.Organizations;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Entities
{
    [MappedTo(typeof(ApplicationOrganizationUser))]
    public class OrganizationUserEntity
    {
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int ID { get; set; }
        public bool IsActive { get; set; }
    }
}

using System;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationOrganizationUser))]
    public class OrganizationUserEntity
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

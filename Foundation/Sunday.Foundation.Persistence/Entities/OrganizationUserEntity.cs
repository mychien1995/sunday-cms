using System;
using Sunday.Core;
using Sunday.Foundation.Domain;

namespace Sunday.Foundation.Persistence.Entities
{
    [MappedTo(typeof(ApplicationOrganizationUser))]
    public class OrganizationUserEntity
    {
        public Guid Id { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

        public OrganizationUserEntity(Guid id, int organizationId, int userId, bool isActive)
        {
            Id = id;
            OrganizationId = organizationId;
            UserId = userId;
            IsActive = isActive;
        }
    }
}

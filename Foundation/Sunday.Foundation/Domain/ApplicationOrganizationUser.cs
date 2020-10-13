using System;

namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganizationUser
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public ApplicationOrganization Organization { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsActive { get; set; }

        public ApplicationOrganizationUser(Guid id, Guid organizationId, Guid userId, ApplicationOrganization organization, ApplicationUser user, bool isActive)
        {
            Id = id;
            OrganizationId = organizationId;
            UserId = userId;
            Organization = organization;
            User = user;
            IsActive = isActive;
        }
    }
}

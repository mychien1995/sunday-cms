namespace Sunday.Foundation.Domain
{
    public class ApplicationOrganizationUser
    {
        public ApplicationOrganization Organization { get; set; }
        public ApplicationUser User { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int ID { get; set; }
        public bool IsActive { get; set; }
    }
}

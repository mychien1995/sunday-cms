namespace Sunday.Core.Domain
{
    public interface IApplicationRole
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}

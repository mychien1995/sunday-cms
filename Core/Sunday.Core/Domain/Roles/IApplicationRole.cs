namespace Sunday.Core.Domain.Roles
{
    public interface IApplicationRole
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}

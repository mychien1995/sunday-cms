namespace Sunday.Core.Domain.FeatureAccess
{
    public interface IApplicationModule
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
    }
}

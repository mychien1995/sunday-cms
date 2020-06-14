namespace Sunday.Core.Domain.FeatureAccess
{
    public interface IApplicationFeature
    {
        public int ID { get; set; }
        public string FeatureName { get; set; }
        public string FeatureCode { get; set; }
        public int ModuleId { get; set; }
        public IApplicationModule Module { get; set; }
    }
}

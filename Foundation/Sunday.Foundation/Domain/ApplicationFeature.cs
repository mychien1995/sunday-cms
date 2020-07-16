namespace Sunday.Foundation.Domain
{
    public class ApplicationFeature
    {
        public int ID { get; set; }
        public string FeatureName { get; set; }
        public string FeatureCode { get; set; }
        public int ModuleId { get; set; }
        public ApplicationModule Module { get; set; }
    }
}

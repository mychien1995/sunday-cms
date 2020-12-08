using System.Xml.Serialization;

namespace Sunday.Core.Configuration.Nodes
{
    public class ServiceNode
    {
        [XmlAttribute("serviceType")]
        public string? ServiceType { get; set; }
        [XmlAttribute("implementationType")]
        public string? ImplementationType { get; set; }
        [XmlAttribute("scope")]
        public string? LifetimeScope { get; set; }
    }
}

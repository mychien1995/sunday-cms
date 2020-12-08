using System.Xml.Serialization;

namespace Sunday.Core.Configuration.Nodes
{
    public class ProcessorNode
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }
    }
}

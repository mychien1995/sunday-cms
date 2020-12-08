using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration.Nodes
{
    public class PipelineNode
    {
        [XmlAttribute("name")] 
        public string Name { get; set; } = string.Empty;
        [XmlElement("processor")]
        public List<ProcessorNode> Processors { get; set; } = new List<ProcessorNode>();
    }
}

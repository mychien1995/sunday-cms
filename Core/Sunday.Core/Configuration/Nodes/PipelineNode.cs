using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration
{
    public class PipelineNode
    {
        public PipelineNode()
        {
            Processors = new List<ProcessorNode>();
        }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("processor")]
        public List<ProcessorNode> Processors { get; set; }
    }
}

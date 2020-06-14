using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration
{
    [XmlRoot("configuration")]
    public class ConfigurationNode
    {
        [XmlArray("pipelines")]
        [XmlArrayItem("pipeline")]
        public List<PipelineNode> Pipelines { get; set; }
        [XmlArray("settings")]
        [XmlArrayItem("setting")]
        public List<SettingNode> Settings { get; set; }

        [XmlArray("services")]
        [XmlArrayItem("services")]
        public List<ServiceNode> Services { get; set; }
    }
}

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration.Nodes
{
    [XmlRoot("configuration")]
    public class ConfigurationNode
    {
        [XmlArray("pipelines")]
        [XmlArrayItem("pipeline")]
        public List<PipelineNode> Pipelines { get; set; } = new List<PipelineNode>();
        [XmlArray("settings")]
        [XmlArrayItem("setting")]
        public List<SettingNode> Settings { get; set; } = new List<SettingNode>();

        [XmlArray("services")]
        [XmlArrayItem("services")]
        public List<ServiceNode> Services { get; set; } = new List<ServiceNode>();
    }
}

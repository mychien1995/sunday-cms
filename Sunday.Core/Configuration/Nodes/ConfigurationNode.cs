using System;
using System.Collections.Generic;
using System.Text;
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
    }
}

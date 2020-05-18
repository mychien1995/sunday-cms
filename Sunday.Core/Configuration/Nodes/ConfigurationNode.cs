using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration
{
    [XmlRoot("configuration")]
    public class ConfigurationNode
    {
        [XmlElement("pipelines")]
        public List<PipelineNode> Pipelines { get; set; }
        [XmlElement("settings")]
        public List<SettingNode> Settings { get; set; }
    }
}

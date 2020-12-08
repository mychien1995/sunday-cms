using System.Xml.Serialization;

namespace Sunday.Core.Configuration.Nodes
{
    public class SettingNode
    {
        [XmlAttribute("name")]
        public string? Key { get; set; }
        [XmlAttribute("value")]
        public string? Value { get; set; }
    }
}

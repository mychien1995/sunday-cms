using System.Xml;
using Sunday.Core.Configuration.Nodes;

namespace Sunday.Core.Configuration
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration(ConfigurationNode confNode, XmlDocument xmlFile)
        {
            ConfigurationNode = confNode;
            ConfigurationXml = xmlFile;
        }
        public ConfigurationNode ConfigurationNode { get; set; }
        public XmlDocument ConfigurationXml { get; set; }
    }
}

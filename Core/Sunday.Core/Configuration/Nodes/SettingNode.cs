using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration
{
    public class SettingNode
    {
        [XmlAttribute("name")]
        public string Key { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}

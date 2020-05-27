using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Sunday.Core.Configuration
{
    public class ProcessorNode
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}

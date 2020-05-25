﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Sunday.Core.Configuration
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration(ConfigurationNode confNode, XmlDocument xmlFile)
        {
            this.ConfigurationNode = confNode;
            this.ConfigurationXml = xmlFile;
        }
        public ConfigurationNode ConfigurationNode { get; set; }
        public XmlDocument ConfigurationXml { get; set; }
    }
}

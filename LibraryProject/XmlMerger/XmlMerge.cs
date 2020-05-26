namespace XmlDocumentMerger
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml;
    public static class XmlMerger
    {
        public static string MergeDocuments(
            string xmlToMergeFrom,
            string xmlToMergeInto)
        {
            if (string.IsNullOrEmpty(xmlToMergeFrom)) return xmlToMergeInto;
            else if (string.IsNullOrEmpty(xmlToMergeInto)) return xmlToMergeFrom;
            else if (string.IsNullOrEmpty(xmlToMergeFrom) &&
                     string.IsNullOrEmpty(xmlToMergeInto)) return xmlToMergeInto;

            // --

            var sourceDoc = new XmlDocument();
            var destDoc = new XmlDocument();

            sourceDoc.LoadXml(xmlToMergeFrom);
            destDoc.LoadXml(xmlToMergeInto);

            // --

            foreach (
                var sourceNode in sourceDoc.ChildNodes.Cast<XmlNode>().Where(n => n.NodeType == XmlNodeType.Element))
            {
                doProcessNode(sourceNode, destDoc);
            }

            // --

            return destDoc.OuterXml;
        }

        private static void doProcessNode(XmlNode sourceNode, XmlNode destParentNode)
        {
            var findNode = FindNode(sourceNode, destParentNode.OwnerDocumentIntelligent());
            if (findNode.Decision == NodeDecision.Copy)
            {
                copyNode(sourceNode, destParentNode);
            }
            else
            {
                var destNode = findNode.Node;
                if (findNode.Decision == NodeDecision.Update)
                {
                    updateNode(sourceNode, destNode);
                }
                foreach (
                    var childNode in sourceNode.ChildNodes.Cast<XmlNode>().Where(n => n.NodeType == XmlNodeType.Element)
                    )
                {
                    doProcessNode(childNode, destNode);
                }
            }
        }

        private static void updateNode(XmlNode sourceNode, XmlNode destNode)
        {
            destNode.InnerText = sourceNode.InnerText;
            for (var i = 0; i < sourceNode.Attributes.Count; i++)
            {
                var aAttribute = sourceNode.Attributes[i];
                if (aAttribute.Name == "patch.replace") continue;
                if (destNode.Attributes[aAttribute.Name] == null)
                {
                    var newAttr = destNode.OwnerDocumentIntelligent().CreateAttribute(aAttribute.Name);
                    newAttr.Value = aAttribute.Value;
                    destNode.Attributes.Append(newAttr);
                }
                else if (destNode.Attributes[aAttribute.Name].Value != aAttribute.Value)
                {
                    destNode.Attributes[aAttribute.Name].Value = aAttribute.Value;
                }
            }
        }
        private static void copyNode(XmlNode sourceNode, XmlNode destParentNode)
        {
            // ReSharper disable once PossibleNullReferenceException
            var newNode = destParentNode.OwnerDocumentIntelligent().ImportNode(sourceNode, true);
            destParentNode.AppendChild(newNode);
        }

        private static FindNodeDecision FindNode(XmlNode sourceNode, XmlNode destDoc)
        {
            var xPath = findXPath(sourceNode);
            var destNode = destDoc.SelectSingleNode(xPath);
            if (destNode == null)
                return new FindNodeDecision(null, NodeDecision.Copy);
            if (!IdenticalNode(sourceNode, destNode))
            {
                if (sourceNode.Attributes["patch.replace"]?.Value == "true")
                {
                    return new FindNodeDecision(destNode, NodeDecision.Update);
                }
                else return new FindNodeDecision(null, NodeDecision.Copy);
            }
            return new FindNodeDecision(destNode, NodeDecision.Continue);
        }

        private static bool IdenticalNode(XmlNode nodeA, XmlNode nodeB)
        {
            if (nodeA.Attributes.Count != nodeB.Attributes.Count) return false;
            for (var i = 0; i < nodeA.Attributes.Count; i++)
            {
                var aAttribute = nodeA.Attributes[i];
                if (nodeB.Attributes[aAttribute.Name] == null || nodeB.Attributes[aAttribute.Name].Value != aAttribute.Value)
                    return false;
            }
            return true;
        }

        // http://stackoverflow.com/a/241291/107625
        private static string findXPath(XmlNode node)
        {
            var builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, string.Format(@"/@{0}", node.Name));
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        var index = findElementIndex((XmlElement)node);
                        builder.Insert(0, string.Format(@"/{0}[{1}]", node.Name, index));
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        private static int findElementIndex(XmlNode element)
        {
            var parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }

            var parent = (XmlElement)parentNode;
            var index = 1;

            if (parent != null)
            {
                foreach (XmlNode candidate in parent.ChildNodes)
                {
                    if (candidate is XmlElement && candidate.Name == element.Name)
                    {
                        if (candidate == element)
                        {
                            return index;
                        }
                        index++;
                    }
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }
    }

    internal static class XmlExtensions
    {
        public static XmlDocument OwnerDocumentIntelligent(this XmlNode node)
        {
            if (node == null) return null;
            else
            {
                var document = node as XmlDocument;
                return document ?? node.OwnerDocument;
            }
        }
    }

    internal class FindNodeDecision
    {
        public XmlNode Node { get; set; }
        public NodeDecision Decision { get; set; }
        public FindNodeDecision(XmlNode node, NodeDecision decision)
        {
            Node = node;
            Decision = decision;
        }
    }

    internal enum NodeDecision
    {
        Copy,
        Continue,
        Update
    }
}
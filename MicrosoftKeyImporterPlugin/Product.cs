using System.Collections.Generic;
using System.Xml;

namespace MicrosoftKeyImporterPlugin
{
    internal class Product
    {
        private readonly XmlNode _node;

        public Product(XmlNode node) => _node = node;

        public string Name => _node.Attributes?["Name"].Value.Replace("\n", string.Empty);

        public IEnumerable<Key> Keys => GetKeys();

        private IEnumerable<Key> GetKeys()
        {
            var keys = new List<Key>();

            foreach (XmlNode key in _node.ChildNodes)
            {
                keys.Add(new Key
                {
                    Value = key.InnerText,
                    Type = key.Attributes != null ? key.Attributes["Type"].Value : string.Empty,
                    Description = key.FirstChild.NodeType == XmlNodeType.CDATA ? key.InnerText : string.Empty
                });
            }

            return keys;
        }
    }
}
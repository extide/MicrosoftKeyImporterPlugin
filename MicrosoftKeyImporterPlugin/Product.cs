using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace MicrosoftKeyImporterPlugin
{
    internal class Product
    {
        private readonly XmlNode _node;

        public Product(XmlNode node) => _node = node;

        public string Name => _node.Attributes?["Name"].Value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        public string KeyRetrievalNote => _node.Attributes?["KeyRetrievalNote"]?.Value;

        public IEnumerable<Key> Keys => GetKeys();

        private IEnumerable<Key> GetKeys()
        {
            var keys = new List<Key>();

            foreach (XmlNode key in _node.ChildNodes)
            {
                //int keyId;
                //int.TryParse(key.Attributes["ID"]?.Value, out keyId);
                //if (keyId < 0)
                //{
                //    Debug.Print("Negative Key: " + HttpUtility.HtmlDecode(key.InnerText));
                //}
                keys.Add(new Key
                {
                    Value = key.FirstChild.NodeType != XmlNodeType.CDATA ? HttpUtility.HtmlDecode(key.InnerText) : string.Empty,
                    ID = key.Attributes["ID"]?.Value,
                    Type = key.Attributes["Type"]?.Value,
                    ClaimedDate = key.Attributes["ClaimedDate"]?.Value,
                    Notes = key.Attributes["Notes"]?.Value,
                    Description = key.FirstChild.NodeType == XmlNodeType.CDATA ? HttpUtility.HtmlDecode(key.InnerText) : string.Empty
                });
            }

            return keys;
        }
    }
}
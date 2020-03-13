using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Xml;

namespace MicrosoftKeyImporterPlugin
{
    internal class Product
    {
        public string Name { get; private set; }
        public string KeyRetrievalNote { get; private set; }
        public IEnumerable<Key> Keys { get; private set; }

        public Product(XmlNode node)
        {
            Name = node.Attributes?["Name"]?.Value?.Trim()?.Replace("\r", string.Empty)?.Replace("\n", string.Empty);
            KeyRetrievalNote = node.Attributes?["KeyRetrievalNote"]?.Value?.Trim();

            Keys = new List<Key>();
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                //int keyid;
                //int.TryParse(xmlNode.Attributes["id"]?.Value, out keyid);
                //if (keyid < 0)
                //{
                //    Debug.Print("negative key: " + HttpUtility.HtmlDecode(xmlNode.InnerText));
                //}
                ((List<Key>)Keys).Add(new Key(xmlNode));
            }
        }      
    }
}
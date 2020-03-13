using System;
using System.Web;
using System.Xml;

namespace MicrosoftKeyImporterPlugin
{
    internal class Key
    {
        private string _claimeddate = string.Empty;

        public string ID { get; private set; }
        public string Value { get; private set; }
        public string Type { get; private set; }
        public string Description { get; private set; }
        public string Notes { get; private set; }

        public string ClaimedDate
        {
            get { return _claimeddate; }
            private set
            {
                DateTime time;
                if (DateTime.TryParse(value, out time))
                    _claimeddate = time.ToString("d");
            }
        }

        public Key(XmlNode xmlNode)
        {
            this.Value = xmlNode.FirstChild?.NodeType != XmlNodeType.CDATA ? HttpUtility.HtmlDecode(xmlNode.InnerText) : string.Empty;
            this.ID = xmlNode.Attributes["ID"]?.Value;
            this.Type = xmlNode.Attributes["Type"]?.Value;
            this.ClaimedDate = xmlNode.Attributes["ClaimedDate"]?.Value;
            this.Notes = xmlNode.Attributes["Notes"]?.Value;
            this.Description = xmlNode.FirstChild?.NodeType == XmlNodeType.CDATA ? HttpUtility.HtmlDecode(xmlNode.InnerText) : string.Empty;
        }
    }
}
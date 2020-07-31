using System;
using System.Globalization;
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
                if (DateTime.TryParse(value, out DateTime time))
                    _claimeddate = time.ToString("d", CultureInfo.CurrentCulture);
            }
        }

        public Key(XmlNode xmlNode)
        {
            Value = xmlNode.FirstChild?.NodeType != XmlNodeType.CDATA ? HttpUtility.HtmlDecode(xmlNode.InnerText?.Trim()) : string.Empty;
            ID = xmlNode.Attributes["ID"]?.Value?.Trim();
            Type = xmlNode.Attributes["Type"]?.Value?.Trim();
            ClaimedDate = xmlNode.Attributes["ClaimedDate"]?.Value?.Trim();
            Notes = xmlNode.Attributes["Notes"]?.Value?.Trim();
            Description = xmlNode.FirstChild?.NodeType == XmlNodeType.CDATA ? HttpUtility.HtmlDecode(xmlNode.InnerText?.Trim()) : string.Empty;
        }
    }
}
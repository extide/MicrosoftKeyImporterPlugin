using System;

namespace MicrosoftKeyImporterPlugin
{
    internal class Key
    {
        private string _claimeddate = string.Empty;

        public string ID { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ClaimedDate
        {
            get { return _claimeddate; }
            set
            {
                DateTime time;
                if (DateTime.TryParse(value, out time))
                    _claimeddate = time.ToString("d");
            }
        }
        public string Notes { get; set; }
    }
}
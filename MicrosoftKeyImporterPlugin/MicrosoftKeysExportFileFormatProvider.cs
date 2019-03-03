using KeePass.DataExchange;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Security;
using System.Xml;

namespace MicrosoftKeyImporterPlugin
{
    internal class MicrosoftKeysExportFileFormatProvider : FileFormatProvider
    {
        public override bool SupportsImport => true;
        public override bool SupportsExport => false;
        public override string FormatName => "MSDN or TechNet Keys XML";
        public override string DefaultExtension => "xml";

        public override void Import(PwDatabase pwStorage, System.IO.Stream sInput, IStatusLogger slLogger)
        {
            var document = new XmlDocument();
            document.Load(sInput);

            XmlElement root = document.DocumentElement;
            XmlNode keys = root?.SelectSingleNode("YourKey");
            XmlNodeList products = keys?.SelectNodes("Product_Key");

            if (products == null || products.Count == 0)
                return;

            PwGroup msdnGroup = pwStorage.RootGroup.FindCreateGroup("Microsoft Product Keys", true);

            for (var i = 0; i < products.Count; i++)
            {
                var product = new Product(products[i]);
                slLogger.SetText($"{product.Name} ({i + 1} of {products.Count})", LogStatusType.Info);
                AddProduct(pwStorage, msdnGroup, product);
            }
        }

        private static void AddProduct(PwDatabase database, PwGroup group, Product product)
        {
            PwGroup productGroup = group.FindCreateGroup(product.Name, true);

            foreach (Key key in product.Keys)
            {
                if (!GroupContainsKeyAsPassword(productGroup, key))
                    AddKey(database, productGroup, key);
            }
        }

        private static void AddKey(PwDatabase database, PwGroup group, Key key)
        {
            var entry = new PwEntry(true, true);

            group.AddEntry(entry, true);

            entry.Strings.Set(PwDefs.TitleField, new ProtectedString(database.MemoryProtection.ProtectTitle, key.Type));
            entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(database.MemoryProtection.ProtectPassword, key.Value));
            entry.Strings.Set(PwDefs.NotesField, new ProtectedString(database.MemoryProtection.ProtectNotes, key.Description));
        }

        private static bool GroupContainsKeyAsPassword(PwGroup group, Key key)
        {
            foreach (PwEntry entry in group.Entries)
            {
                if (key.Value == entry.Strings.Get(PwDefs.PasswordField).ReadString())
                    return true;
            }
            return false;
        }
    }
}
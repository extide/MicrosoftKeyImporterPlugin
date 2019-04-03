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
            XmlDocument document = new XmlDocument();
            document.Load(sInput);

            XmlElement root = document.DocumentElement;
            XmlNodeList products;

            if (root.Name == "YourKey")
                products = root?.SelectNodes("Product_Key");
            else
            {
                XmlNode keys = root?.SelectSingleNode("YourKey");
                products = keys?.SelectNodes("Product_Key");
            }

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
                    AddKey(database, productGroup, product, key);
            }
        }

        private static void AddKey(PwDatabase database, PwGroup group, Product product, Key key)
        {
            PwEntry entry = new PwEntry(true, true);
            group.AddEntry(entry, true);

            string note = (string.IsNullOrEmpty(key.ClaimedDate) ? "" : $"Claimed on : {key.ClaimedDate}\n\n")
                + (string.IsNullOrEmpty(product.KeyRetrievalNote) ? "" : $"Key Retrieval Note : {product.KeyRetrievalNote}\n\n")
                + (string.IsNullOrEmpty(key.Description) ? "" : $"Description : \n\n{key.Description}");

            entry.Strings.Set(PwDefs.TitleField, new ProtectedString(database.MemoryProtection.ProtectTitle, key.Type));
            entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(database.MemoryProtection.ProtectPassword, key.Value.Trim()));
            entry.Strings.Set(PwDefs.NotesField, new ProtectedString(database.MemoryProtection.ProtectNotes, note));
            entry.Strings.Set("Product Name", new ProtectedString(true, product.Name));
        }

        private static bool GroupContainsKeyAsPassword(PwGroup group, Key key)
        {
            foreach (PwEntry entry in group.Entries)
            {
                if (key.Value.Trim() == entry.Strings.Get(PwDefs.PasswordField).ReadString().Trim())
                    return true;
            }
            return false;
        }
    }
}
using KeePass.Plugins;
using KeePass.Util;
using MicrosoftKeyImporterPlugin.Properties;
using System.Drawing;

namespace MicrosoftKeyImporterPlugin
{
    public sealed class MicrosoftKeyImporterPluginExt : Plugin
    {
        private IPluginHost _host;
        private MicrosoftKeysExportFileFormatProvider _provider;
        public override Image SmallIcon => Resources.Icon;
        public override string UpdateUrl => "";

        public string PublicKey { get; } = "";

        public override bool Initialize(IPluginHost host)
        {
            _host = host;
            _provider = new MicrosoftKeysExportFileFormatProvider();

            _host?.FileFormatPool.Add(_provider);

            UpdateCheckEx.SetFileSigKey(UpdateUrl, PublicKey);

            return true;
        }

        public override void Terminate()
        {
            _host.FileFormatPool.Remove(_provider);
        }
    }
}
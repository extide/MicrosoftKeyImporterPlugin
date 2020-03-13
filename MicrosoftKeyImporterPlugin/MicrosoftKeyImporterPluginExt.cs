using KeePass.Plugins;
using MicrosoftKeyImporterPlugin.Properties;
using System.Drawing;
using System.Resources;

namespace MicrosoftKeyImporterPlugin
{
    public sealed class MicrosoftKeyImporterPluginExt : Plugin
    {
        private IPluginHost _host;
        private MicrosoftKeysExportFileFormatProvider _provider;
        public override Image SmallIcon => Resources.Icon;

        public override bool Initialize(IPluginHost host)
        {
            _host = host;
            _provider = new MicrosoftKeysExportFileFormatProvider();

            _host.FileFormatPool.Add(_provider);
            
            return true;
        }

        public override void Terminate()
        {
            _host.FileFormatPool.Remove(_provider);
        }
    }
}
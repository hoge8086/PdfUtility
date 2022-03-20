using PanelPluginInterface;
using System.Windows.Controls;

namespace PdfUtility.Plugins
{
    public class CreatePdfPanelPlugin : IPanelPlugin
    {
        private IPluginHost host;
        UserControl panel;
        public string Name => "PDF化";

        public UserControl Panel => panel;

        public IPluginHost Host {
            get => host;
            set => host = value;
        }

        public CreatePdfPanelPlugin(IPluginHost host)
        {
             panel = new CreatePdfPanel(host);
        }
    }
}

using PanelPluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MakePdfPlugin
{
    public class SearchPdfPanelPlugin : IPanelPlugin
    {
        private IPluginHost host;
        UserControl panel;
        public string Name => "PDF検索(ページ単位)";

        public UserControl Panel => panel;

        public IPluginHost Host {
            get => host;
            set => host = value;
        }

        public SearchPdfPanelPlugin(IPluginHost host)
        {
             panel = new PdfSearchPanel(host);
        }
    }
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
             panel = new MakePdfPanel(host);
        }
    }
}

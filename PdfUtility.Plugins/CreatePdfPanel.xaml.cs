using PanelPluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfUtility.Plugins
{
    /// <summary>
    /// MakePdfPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class CreatePdfPanel : UserControl, IPanelPlugin
    {
        public string PluginName => "PDF化";

        public UserControl Panel => this;
        public CreatePdfPanel(IPluginHost host)
        {
            InitializeComponent();
            var dir = System.IO.Directory.GetCurrentDirectory();
            this.DataContext = new CreatePdfPanelViewModel(
                host,
                System.IO.Path.Combine(dir, "out"),
                System.IO.Path.Combine(dir, "temp"));
        }
    }
}

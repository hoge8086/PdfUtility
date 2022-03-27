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
using PanelPluginInterface;

namespace PdfUtility.Plugins
{
    /// <summary>
    /// PdfSearchPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchPdfPanel : UserControl, IPanelPlugin
    {
        public string PluginName => "PDF検索";

        public UserControl Panel => this;

        public SearchPdfPanel(IPluginHost host)
        {
            InitializeComponent();
            this.DataContext = new SearchPdfPanelViewModel(
                host, 
                (string pdfPath, int page) =>
                {
                    var browser = new BrowsPdfWindow();
                    browser.Owner = Window.GetWindow(this);
                    browser.Show();
                    browser.ShowPdf(pdfPath, page);
                });
        }
    }
}

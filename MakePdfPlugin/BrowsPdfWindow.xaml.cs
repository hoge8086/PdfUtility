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
using System.Windows.Shapes;

namespace MakePdfPlugin
{
    /// <summary>
    /// BrowsPdfWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BrowsPdfWindow : Window
    {
        public BrowsPdfWindow()
        {
            InitializeComponent();
        }

        public void ShowPdf(string pdfPath, int page)
        {
            // 参考<https://www.doraxdora.com/2017/11/02/post-2971/>
            var url = new Uri($"file://{pdfPath}#page={page}");
            // MEMO:urlをstringで渡した場合、日本語語が含まれるとなぜか文字化けする
            webBrowser.Navigate(url);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

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

namespace MakePdfPlugin
{
    /// <summary>
    /// MakePdfPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class MakePdfPanel : UserControl
    {
        public MakePdfPanel()
        {
            InitializeComponent();
            var dir = System.IO.Directory.GetCurrentDirectory();;
            this.DataContext = new MakePdfPanelViewModel(
                System.IO.Path.Combine(dir, "out"),
                System.IO.Path.Combine(dir, "temp"));
        }
    }
}

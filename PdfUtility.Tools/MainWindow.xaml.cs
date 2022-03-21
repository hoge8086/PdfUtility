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

namespace PdfUtility.Tools
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        PluginManager pluginManager;
        public MainWindow()
        {
            InitializeComponent();
            pluginManager = new PluginManager();
            try
            {
                pluginManager.LoadPlugins();
                foreach(var plugin in pluginManager.Plugins)
                {
                    try
                    {
                        var item = new TabItem();
                        item.Header = plugin.Instance.PluginName;
                        item.Content = plugin.Instance.Panel;
                        pluginsTabControl.Items.Add(item);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}

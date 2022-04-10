using System;
using System.Collections.ObjectModel;
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

        public class Log
        {
            public DateTime DateTime { get; } = DateTime.Now;
            public string Content { get; }
            public Log(string content)
            {
                Content = content;
            }
        }

        public ObservableCollection<Log> LogList { get; } = new ObservableCollection<Log>();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = LogList;
            pluginManager = new PluginManager((m) => {
                Dispatcher.Invoke(() =>
                {
                    LogList.Add(new Log(m));
                });
            });
            var jsonRepo = new JsonRepository<PluginConfig>(@".\plugin_config.json");
            var config = jsonRepo.Load();
            pluginManager.LoadPlugins();
            var orderdPlugins = pluginManager.Plugins.OrderBy(x => GetTabOrderPriority(x, config, pluginManager.Plugins.Count));
            try
            {
                foreach(var plugin in orderdPlugins)
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

            this.Closed += (s, e) =>
            {
                // TODO:タブ移動対応した場合はUIの順で保存する必要あり
                config.PluginTabOrder = orderdPlugins.Select(x => x.ClassName).ToList();
                jsonRepo.Save(config);
            };

            int GetTabOrderPriority(PanelPlugin plugin, PluginConfig cfg, int max)
            {
                var index = cfg.PluginTabOrder.FindIndex(x => x == plugin.ClassName);
                if (index < 0)
                    return max;
                return index;
            }
        }
    }
}

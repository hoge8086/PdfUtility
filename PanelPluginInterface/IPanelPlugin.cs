using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PanelPluginInterface
{
    public interface IPluginHost
    {
        void SaveSetting(object setting);
        void LoadSetting(Type type, object setting);
        string GetCurrentDirectory();

    }
    public interface IPanelPlugin
    {
        string Name { get; }
        UserControl Panel { get; }
        IPluginHost Host { get; set; }
    }
}

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
        void Log(string message);

    }
    public interface IPanelPlugin
    {
        string PluginName { get; }
        UserControl Panel { get; }
    }
}

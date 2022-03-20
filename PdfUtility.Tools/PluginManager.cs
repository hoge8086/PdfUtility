using PanelPluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfUtility.Tools
{
    public class PluginManager : IPluginHost
    {
        public List<PanelPlugin> Plugins;

        public PluginManager()
        {
            Plugins = new List<PanelPlugin>();
        }

        string IPluginHost.GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        void IPluginHost.LoadSetting(Type type, object setting)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.SaveSetting(object setting)
        {
            throw new NotImplementedException();
        }

        public void LoadPlugins()
        {
            string ipluginName = typeof(IPanelPlugin).FullName;
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string[] dlls = System.IO.Directory.GetFiles(currentDirectory, "*.dll", SearchOption.AllDirectories);

            foreach (string dll in dlls)
            {
                try
                {
                    //アセンブリとして読み込む
                    var asm = Assembly.LoadFrom(dll);
                    foreach (Type t in asm.GetTypes())
                    {
                        //アセンブリ内のすべての型について、
                        //プラグインとして有効か調べる
                        if (t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface(ipluginName) != null)
                        {
                            //PluginInfoをコレクションに追加する
                            Plugins.Add(new PanelPlugin(dll, t.FullName, this));
                        }
                    }
                }
                catch
                {
                }
            }
        }

    }

    public class PanelPlugin
    {
        public string Location { get; }
        public string ClassName { get; }
        public IPanelPlugin Instance { get; }
        public PanelPlugin(string path, string cls, IPluginHost host)
        {
            this.Location = path;
            this.ClassName = cls;

            Assembly asm = Assembly.LoadFrom(this.Location);
            //クラス名からインスタンスを作成する
            Instance = (IPanelPlugin) asm.CreateInstance(
                this.ClassName,
                false,
                BindingFlags.CreateInstance,
                null,
                new object[] { host },
                null,
                null);
        }
    }
}

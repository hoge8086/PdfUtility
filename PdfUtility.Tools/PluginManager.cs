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

        public Action<string> LogFunc;

        public PluginManager(Action<string> log)
        {
            Plugins = new List<PanelPlugin>();
            LogFunc = log;
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
                            try
                            {
                                //PluginInfoをコレクションに追加する
                                Plugins.Add(new PanelPlugin(dll, t.FullName, this));
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }
        }

        public void Log(string message)
        {
            LogFunc?.Invoke(message);
        }
    }

    public class PanelPlugin
    {
        public string AssemblyPath { get; }
        public string ClassName { get; }
        public IPanelPlugin Instance { get; }
        public PanelPlugin(string path, string className, IPluginHost host)
        {
            this.AssemblyPath = path;
            this.ClassName = className;

            Assembly asm = Assembly.LoadFrom(this.AssemblyPath);
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

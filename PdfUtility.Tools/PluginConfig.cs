using System.Collections.Generic;

namespace PdfUtility.Tools
{
    public class PluginConfig
    {
        public List<string> PluginTabOrder { get; set; }
        public PluginConfig()
        {
            PluginTabOrder = new List<string>();
        }
    }
}

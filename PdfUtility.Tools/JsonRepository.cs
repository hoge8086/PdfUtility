using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace PdfUtility.Tools
{
    public class JsonRepository<T> where T  : new()
    {
        private string jsonPath;

        public JsonRepository(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        public T Load()
        {
            try
            {
                string jsonText = File.ReadAllText(jsonPath, Encoding.UTF8);
                return JsonSerializer.Deserialize<T>(jsonText);
            }
            catch (Exception ex) { }

            return new T();
        }

        public void Save(T obj)
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
                    WriteIndented = true,
                };
                using (var stream = File.Create(jsonPath))
                using (StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    string jsonText = JsonSerializer.Serialize(obj, options);
                    writer.Write(jsonText);
                }

            }
            catch (Exception ex) { }
       }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfUtility
{
    public class TempDirectory
    {
        public string Path { get; private set; }

        public TempDirectory(string dirPath)
        {
            this.Path = dirPath;
            if (Directory.Exists(Path))
                DeleteAllFiles();
            else
                Directory.CreateDirectory(Path);

        }

        public string CopyFile(string fromFilePath)
        {
            string tempFilePath = CreateFilePath(System.IO.Path.GetFileName(fromFilePath));
            File.Copy(fromFilePath, tempFilePath, true);
            return tempFilePath;
        }

        private string CreateFilePath(string fineName)
        {
            return System.IO.Path.Combine(Path, fineName);
        }

        //public void Create()
        //{
        //    if (!Directory.Exists(Path))
        //        Directory.CreateDirectory(Path);
        //}

        public void DeleteAllFiles()
        {
            foreach(var file in Directory.GetFiles(Path))
                File.Delete(file);
        }

        public void Delete()
        {
            DeleteAllFiles();
            Directory.Delete(Path);
        }
    }
}

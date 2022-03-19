using System;
using System.Collections.Generic;
using System.IO;

namespace PdfUtility.Business
{
    public class FileUtility
    {
        public static void CheckExistFiles(List<string> filePaths)
        {
            foreach (var fromFilePath in filePaths)
                if (!ExistPathRooted(fromFilePath))
                    throw new ArgumentException(fromFilePath + "が存在しません. 絶対パスを指定してください.");
        }

        public static bool IsExtension(string filePath, string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            var fileExtension = Path.GetExtension(filePath);
            return fileExtension.Equals(extension, StringComparison.OrdinalIgnoreCase);
        }
        public static bool ExistPathRooted(string path)
        {
            if (!string.IsNullOrEmpty(path) ||
               System.IO.Path.IsPathRooted(path) ||
               Directory.Exists(path))
                return true;

            return false;
        }
    }
}

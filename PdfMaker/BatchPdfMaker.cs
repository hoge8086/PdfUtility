using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;

namespace PdfMaker
{
    public class BatchPdfMaker

    {
        private TempDirectory tempDir;
        public BatchPdfMaker(string tempDirPath)
        {
            tempDir = new TempDirectory(tempDirPath);
        }

        public void MakePdfAndMerge(List<string> filePaths, string outputPdfPath, List<string> keywords)
        {
            if(string.IsNullOrEmpty(outputPdfPath))
                throw new ArgumentException("出力ファイル名が不正です.");
            if (!IsPdfExtension(outputPdfPath))
                outputPdfPath += ".pdf";

            CheckExistFiles(filePaths);
            tempDir.DeleteAllFiles();

            var joiner = new PdfJoiner();
            for(int i=0; i<filePaths.Count; i++)
            {
                var fromFilePath = filePaths[i];
                if(IsPdfExtension(fromFilePath))
                {
                    joiner.Append(fromFilePath);
                }
                else
                {
                    string tempPdfPath = PdfMaker.MakePdfSafelyOriginFile(fromFilePath, tempDir.Path, tempDir);
                    joiner.Append(tempPdfPath);
                }
            }

            joiner.JoinPdf(outputPdfPath);
            PdfMaker.TrimPage(outputPdfPath, keywords, tempDir);
            tempDir.DeleteAllFiles();
        }

        public void MakePdfs(List<string> filePaths, string outputDirectoryPath, List<string> keywords)
        {
            if (!ExistPathRooted(outputDirectoryPath))
            {
                throw new ArgumentException("出力先フォルダが存在しない、あるいはパスが不正です。絶対パスを指定してください.");
            }

            CheckExistFiles(filePaths);
            tempDir.DeleteAllFiles();
            foreach (var fromFilePath in filePaths)
            {
                var path = PdfMaker.MakePdfSafelyOriginFile(fromFilePath, outputDirectoryPath, tempDir);
                PdfMaker.TrimPage(path, keywords, tempDir);
            }
            tempDir.DeleteAllFiles();
        }

        private static void CheckExistFiles(List<string> filePaths)
        {
            foreach (var fromFilePath in filePaths)
                if (!ExistPathRooted(fromFilePath))
                    throw new ArgumentException(fromFilePath + "が存在しません. 絶対パスを指定してください.");
        }

        static private bool IsPdfExtension(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }
        static private bool ExistPathRooted(string path)
        {
            if (!string.IsNullOrEmpty(path) ||
               System.IO.Path.IsPathRooted(path) ||
               Directory.Exists(path))
                return true;

            return false;
        }

    }

}

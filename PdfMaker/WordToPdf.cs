using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Word;
using System.IO;
using System.Runtime.InteropServices;

namespace PdfUtility
{
    public class WordToPdf : IPdfMaker
    {
        public void makePdf(string fromPath, string toPath)
        {
            Microsoft.Office.Interop.Word.Application objWord = null;
            Microsoft.Office.Interop.Word.Documents objDocuments = null;
            Microsoft.Office.Interop.Word.Document objDocument = null;
            // ファイル名を定義する
            try
            {
                // ファイルパスを初期化する
                // Wordオブジェクトを実体化する
                objWord = new Microsoft.Office.Interop.Word.Application();
                objWord.Visible = false;
                // 文章を管理しているオブジェクトを取得する
                objDocuments = objWord.Documents;
                // PDFに変換するWord文章を開く
                objDocument = objDocuments.Open(fromPath);
                // PDF形式で保存する
                objDocument.ExportAsFixedFormat(toPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                objDocument.Close(WdSaveOptions.wdDoNotSaveChanges);
            }
            finally
            {
                // 例外が発生しても必ずWordの文章を閉じるように処理はここ
                if (objDocument != null)
                {
                    Marshal.ReleaseComObject(objDocument);
                    objDocument = null;
                }
                // 例外が発生しても必ずWordを狩猟するように処理はここ
                if (objWord != null)
                {
                    GC.Collect();
                    objWord.Quit();
                    Marshal.ReleaseComObject(objWord);
                    objWord = null;
                }
                GC.Collect();
            }
        }

        public bool isSupported(string filePath)
        {
            var supportedExtensions = new string[]{ ".doc", ".docx" };
            string extension = Path.GetExtension(filePath);
            return supportedExtensions.Contains(extension);
        }
    }
}

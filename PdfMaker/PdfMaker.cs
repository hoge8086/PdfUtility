using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

// TODO:名前をPDFのユーティリティーっぽくする
namespace PdfUtility
{
    /// <summary>
    /// [Window不具合]
    ///   Windowsの[通常使うプリンター]を非PSプリンターに設定しないと、
    ///   OfficeファイルのPDFサイズがバラバラに出力される可能性がある.
    ///   コンパネの[デバイスとプリンター]から[Microsoft XPS Document Writer]や[Microsoft Print to PDF]などに設定する.
    ///   ※MSWの共有プリンタとかだとダメ
    ///   参考:<https://answers.microsoft.com/ja-jp/office/forum/office_2010-excel/excel2010%E3%81%AEpdfmaker/4b231035-8b03-4c8e-b789-f16a72a82aad>
    /// </summary>
    public class PdfMaker
    {
        private static IPdfMaker[] pdfMakers = { new ExcelToPdf(), new WordToPdf() };

        /// <summary>
        /// 同名のPDFファイルを出力フォルダに出力する
        /// ※元のファイルに変更を加えないよう、一時フォルダにコピーしたファイルを使って、PDF出力する
        /// </summary>
        /// <param name="fromFilePath"></param>
        /// <param name="toDirPath"></param>
        /// <param name="tempDir"></param>
        /// <returns></returns>
        public static string MakePdfSafelyOriginFile(string fromFilePath, string toDirPath, TempDirectory tempDir)
        {
            string fileName = Path.GetFileNameWithoutExtension(fromFilePath);
            string toPdfPath = Path.Combine(toDirPath, fileName + ".pdf");
            string tempFilePath = tempDir.CopyFile(fromFilePath);
            MakePdf(tempFilePath, toPdfPath);
            return toPdfPath;
        }

        public static void TrimPage(string pdfPath,  List<string> keywords, TempDirectory tempDir)
        {
            if((keywords != null) && (keywords.Count() != 0))
            {
                string fileName = Path.GetFileNameWithoutExtension(pdfPath);
                string trimedPdfPath = Path.Combine(tempDir.Path, fileName + ".trim.pdf");
                PdfUtils.TrimPageIfNotContainsKeywords(pdfPath, trimedPdfPath, keywords);

                //System.Threading.Thread.Sleep(100);
                File.Delete(pdfPath);
                File.Move(trimedPdfPath, pdfPath);
            }
        }

        public static void MakePdf(string fromFilePath, string toPdfPath)
        {
            foreach (var maker in pdfMakers)
            {
                if (maker.isSupported(fromFilePath))
                {
                    maker.makePdf(fromFilePath, toPdfPath);
                    return;
                }
            }

            throw new InvalidOperationException(fromFilePath + "はPDFファイルを作成できません.");
        }

    }
}

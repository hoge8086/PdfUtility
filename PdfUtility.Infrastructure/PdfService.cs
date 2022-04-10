using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfUtility.Infrastructure
{
    public class PdfService
    {
        public void Join(List<string> srcPdfPathList, string destPdfPath)
        {
            Document objITextDoc = null;
            PdfCopy objPDFCopy = null;

            try
            {
                if (srcPdfPathList.Count < 1)
                    throw new ArgumentException("結合するPDFファイルがありません.");

                objITextDoc = new Document();
                objPDFCopy = new PdfCopy(objITextDoc, new FileStream(destPdfPath, FileMode.Create));

                objITextDoc.Open();

                // 出力するPDFのプロパティを設定
                //objITextDoc.AddKeywords("キーワードです。");
                //objITextDoc.AddAuthor  ("zero0nine.com");
                //objITextDoc.AddTitle   ("結合したPDFファイルです。");
                //objITextDoc.AddCreator ("PDFファイル結合くん");
                //objITextDoc.AddSubject ("結合したPDFファイル");

                // ソートが必要ない場合は、コメント
                //sStrList.Sort();

                // 結合対象ファイル分ループ
                foreach (var fromPath in srcPdfPathList)
                {
                    PdfReader objPdfReader = new PdfReader(fromPath); // 結合元のPDFファイル読込
                    objPDFCopy.AddDocument(objPdfReader); // 結合元のPDFファイルを追加（全ページ）
                    objPdfReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("pdfの結合に失敗しました.\n" + ex.Message);
            }
            finally
            {
                if (objITextDoc != null)
                    objITextDoc.Close();

                if (objPDFCopy != null)
                    objPDFCopy.Close();
            }
        }

        public void ConvertToPdf(string srcFilePath, string destPdfPath)
        {
            IPdfConverter[] pdfMakers = { new ExcelToPdf(), new WordToPdf() };
            foreach (var maker in pdfMakers)
            {
                if (maker.IsSupported(srcFilePath))
                {
                    maker.ConvertToPdf(srcFilePath, destPdfPath);
                    return;
                }
            }

            throw new InvalidOperationException($"拡張子{System.IO.Path.GetExtension(srcFilePath).ToUpper()}のPDF化には対応していません.");
        }

        public void ExtractPages(List<int> extractedPages, string srcPdfPath, string destPdfPath)
        {
            using (PdfReader pdfReader = new PdfReader(srcPdfPath))
            {
                pdfReader.SelectPages(string.Join(",", extractedPages.Select(x => x.ToString()).ToArray()));
                using (var st = new System.IO.FileStream(destPdfPath, System.IO.FileMode.CreateNew))
                {
                    using (PdfStamper pdfStamper = new PdfStamper(pdfReader, st))
                    {
                        pdfStamper.Close();
                    }
                }
            }
        }

        public PdfPages GetPages(string pdfPath)
        {
            var pages = new List<Page>();
            using (var pdfReader = new PdfReader(pdfPath))
            {
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    //ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    ITextExtractionStrategy strategy = new JapaneseTextExtractionStrategy();

                    string bodyText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    pages.Add(new Page(page, bodyText));
                }
                pdfReader.Close();
            }
            return new PdfPages(pages);
        }
    }
}

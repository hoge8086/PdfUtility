using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfMaker
{
    public class PdfPageTrimmer
    {
        static public void Trim(string srcPdfPath, string destPdfPath, List<int> pages)
        {
            using (PdfReader pdfReader = new PdfReader(srcPdfPath))
            {
                pdfReader.SelectPages(string.Join(",", pages.Select(x => x.ToString()).ToArray()));
                using (var st = new System.IO.FileStream(destPdfPath, System.IO.FileMode.CreateNew))
                {
                    using (PdfStamper pdfStamper = new PdfStamper(pdfReader, st))
                    {
                        pdfStamper.Close();
                    }
                }
            }
        }

        static public void TrimByKeyword(string srcPdfPath, string destPdfPath, List<string> keywords)
        {
            // キーワード検索:<https://ja.ojit.com/so/c%23/3477374>
            List<int> pages = new List<int>();
            using (var pdfReader = new PdfReader(srcPdfPath))
            {

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    if (keywords.Any(k => (currentPageText.IndexOf(k, StringComparison.OrdinalIgnoreCase) != -1)))
                    {
                        pages.Add(page);
                    }
                }
                pdfReader.Close();
            }

            Trim(srcPdfPath, destPdfPath, pages);
        }
    }
}

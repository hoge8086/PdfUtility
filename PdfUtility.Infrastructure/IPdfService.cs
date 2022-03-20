using System.Collections.Generic;
using System.Linq;

namespace PdfUtility.Infrastructure
{
    public class Page
    {
        public int PageNumber;
        public string BodyText;

        public Page(int pageNumber, string bodyText)
        {
            PageNumber = pageNumber;
            BodyText = bodyText;
        }
    }

    public class PdfPages
    {
        public List<Page> Pages;

        public int NumberOfPages => Pages.Count();

        public PdfPages(List<Page> pages)
        {
            Pages = pages;
        }

    }

    public interface IPdfService
    {
        void ConvertToPdf(string srcFilePath, string destPdfPath);
        void ExtractPages(List<int> extractedPages, string srcPdfPath, string destPdfPath);
        PdfPages GetPages(string pdfPath);
        void Join(List<string> srcPdfPathList, string destPdfPath);
    }
}
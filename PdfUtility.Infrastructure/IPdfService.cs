using System;
using System.Collections.Generic;
using System.Linq;

namespace PdfUtility.Infrastructure
{
    public class Page
    {
        public int PageNumber { get; }
        public string BodyText { get; }

        public Page(int pageNumber, string bodyText)
        {
            PageNumber = pageNumber;
            BodyText = bodyText;
        }

        public string GetLine(int index)
        {
            var crlf = new char[] { '\n', '\r' };
            var indexOfBeginLine = BodyText.LastIndexOfAny(crlf, index);
            if (indexOfBeginLine == -1)
                indexOfBeginLine = 0;
            else
                indexOfBeginLine += 1;

            var indexOfEndLine = BodyText.IndexOfAny(crlf, index);
            if(indexOfEndLine == -1)
                indexOfEndLine = BodyText.Length - 1;

            return BodyText.Substring(indexOfBeginLine, indexOfEndLine - indexOfBeginLine);
        }

        public int GetLineNumber(int index)
        {
            return BodyText.Substring(0, index).CountOf("\n");
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
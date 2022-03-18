using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfUtility
{
    public class SearchHit
    {
        public int Page;
        public int LineOrIndex;
        public string Text;

        public SearchHit(int page, int lineOrIndex, string text)
        {
            Page = page;
            LineOrIndex = lineOrIndex;
            Text = text;
        }
    }
    
    public class SearchTarget
    {
        public string Keword;
        public bool EnableRegexp;
        public bool IgnoreCase;
        public List<SearchHit> Hits;

        public SearchTarget(string keword, bool enableRegexp, bool ignoreCase)
        {
            Keword = keword;
            EnableRegexp = enableRegexp;
            IgnoreCase = ignoreCase;
            Hits = new List<SearchHit>();
        }

        public string RegexpKeword {
            get
            {
                if (EnableRegexp)
                    return Keword;
                return Regex.Escape(Keword);
            }
        }
        public RegexOptions RegexOptions
        {
            get
            {
                if (IgnoreCase)
                    return RegexOptions.IgnoreCase;

                return RegexOptions.None;

            }
        }
    }
    public class PdfUtils
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

        static public void TrimPageIfNotContainsKeywords(string srcPdfPath, string destPdfPath, List<string> keywords)
        {
            // キーワード検索:<https://ja.ojit.com/so/c%23/3477374>
            //List<int> pages = new List<int>();
            //using (var pdfReader = new PdfReader(srcPdfPath))
            //{

            //    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            //    {
            //        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

            //        string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
            //        if (keywords.Any(k => (currentPageText.IndexOf(k, StringComparison.OrdinalIgnoreCase) != -1)))
            //        {
            //            pages.Add(page);
            //        }
            //    }
            //    pdfReader.Close();
            //}
            var index = CreatePageIndex(srcPdfPath);
            var trimPages = GetPagesContainingKewors(keywords, index);
            Trim(srcPdfPath, destPdfPath, trimPages);
        }
        static private List<int> GetPagesContainingKewors(List<string> keywords, Dictionary<int, string> pageIndex)
        {
            return pageIndex.Keys.Where(page => keywords.Any(k => (pageIndex[page].IndexOf(k, StringComparison.OrdinalIgnoreCase) != -1))).ToList();
        }

        static public int GetPageNum(string pdfPath)
        {
            using (var pdfReader = new PdfReader(pdfPath))
            {
                return pdfReader.NumberOfPages;
            }
        }
        static private Dictionary<int, string> CreatePageIndex(string pdfPath)
        {
            var index = new Dictionary<int, string>();
            using (var pdfReader = new PdfReader(pdfPath))
            {
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    //ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    ITextExtractionStrategy strategy = new JapaneseTextExtractionStrategy();

                    string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    index.Add(page, currentPageText);
                }
                pdfReader.Close();
            }
            return index;
        }


        static private void Search(List<SearchTarget> searchTargets, Dictionary<int, string> pageIndex)
        {
            foreach(var target in searchTargets)
            {
                if (string.IsNullOrEmpty(target.Keword))
                    continue;

                target.Hits = new List<SearchHit>();
                foreach(var page in pageIndex.Keys)
                {
                    MatchCollection results = Regex.Matches(pageIndex[page], target.RegexpKeword, target.RegexOptions);
                    foreach (Match m in results)
                    {
                        target.Hits.Add(new SearchHit(page, m.Index, m.Value));
                    }
                }
            }
        }

        static public void Search(List<SearchTarget> searchTargets, string pdfPath)
        {
            var index = CreatePageIndex(pdfPath);
            Search(searchTargets, index);
        }
    }
}

using PdfUtility.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfUtility.Business
{
    public class SearchHit
    {
        public int Page;
        public int LineNumber;
        public int TotalNumberOfLines;
        public string Word;
        public string Line;

        public SearchHit(int page, string word, string line, int lineNumber, int totalNumberOfLines)
        {
            Page = page;
            Word = word;
            Line = line;
            LineNumber = lineNumber;
            TotalNumberOfLines = totalNumberOfLines;
        }
    }
    
    public class SearchTarget
    {
        public string Keyword;
        public bool EnableRegexp;
        public bool IgnoreCase;
        public List<SearchHit> Hits;

        public SearchTarget(string keyword, bool enableRegexp, bool ignoreCase)
        {
            Keyword = keyword;
            EnableRegexp = enableRegexp;
            IgnoreCase = ignoreCase;
            Hits = new List<SearchHit>();
        }

        public string RegexpKeword {
            get
            {
                if (EnableRegexp)
                    return Keyword;
                return Regex.Escape(Keyword);
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
    public class SearchPdfService
    {

        public void Search(List<SearchTarget> searchTargets, string pdfPath)
        {
            var pdfService = new PdfService();
            var pages = pdfService.GetPages(pdfPath);

            foreach(var target in searchTargets)
            {
                if (string.IsNullOrEmpty(target.Keyword))
                    continue;

                target.Hits = new List<SearchHit>();
                foreach(var page in pages.Pages)
                {
                    MatchCollection results = Regex.Matches(page.BodyText, target.RegexpKeword, target.RegexOptions);

                    foreach (Match m in results)
                    {
                        target.Hits.Add(new SearchHit(
                            page.PageNumber,
                            m.Value,
                            page.GetLine(m.Index),
                            page.GetLineNumber(m.Index),
                            page.NumberOfLines));
                    }
                }
            }
        }
    }
}

using System;
using System.Reactive;
using PanelPluginInterface;
using PdfUtility.Business;
using PdfUtility.Infrastructure;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PdfUtility.Plugins
{
    public class PageResults
    {
        public int Page { get; set; }
        public List<string> Words { get; set; }
    }

    public class SearchKeyword
    {
        public int Number { get; set; }
        public string Word { get; set; } = "";
        public bool EnableRegexp { get; set; } = false;
    }
    public class SearchPdfPanelViewModel
    {
        public IPluginHost Host;
        public ReactiveProperty<string> PdfFilePath { get; private set; }
        public ObservableCollection<SearchKeyword> Keywords { get; private set; }
        public ObservableCollection<PageResults> Results { get; private set; }
        public ReactiveCommand SearchCommand { get; }
        public ReactiveCommand<PageResults> ShowPdfCommand { get; }
        public Action<string, int>  ShowPdf{ get; set; }

        private SearchPdfService searchPdfService = new SearchPdfService();
        private PdfService pdfService = new PdfService();

        public SearchPdfPanelViewModel(IPluginHost host)
        {
            Host = host;
            PdfFilePath = new ReactiveProperty<string>();
            Keywords = new ObservableCollection<SearchKeyword>(Enumerable.Range(1, 5).Select(x => new SearchKeyword() { Number = x }));
            Results = new ObservableCollection<PageResults>();
            SearchCommand = new ReactiveCommand();
            SearchCommand.Subscribe(() =>
            {
                try
                {
                    var targets = Keywords.Select(x => new SearchTarget(x.Word, x.EnableRegexp, true)).ToList();
                    searchPdfService.Search(targets, PdfFilePath.Value);
                    var pageNum = pdfService.GetPages(PdfFilePath.Value).NumberOfPages;

                    Results.Clear();
                    for(int p=1; p<=pageNum; p++)
                    {
                        Results.Add(new PageResults()
                        {
                            Page = p,
                            Words = targets.Select(t => {
                                var hits = t.Hits.Where(x => x.Page == p).Select(x => x.Word);

                                if (hits.Count() == 0)
                                    return "";

                                if (!t.EnableRegexp)
                                    return t.Hits[0].Word;

                                return String.Join(",", t.Hits.Where(x => x.Page == p).Select(x => x.Word));
                            }).ToList(),
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー");
                }
            });
            ShowPdfCommand = new ReactiveCommand<PageResults>();
            ShowPdfCommand.Subscribe((x) =>
            {
                try
                {
                    ShowPdf(PdfFilePath.Value, x.Page);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー");
                }
            });
        }
    }
}
